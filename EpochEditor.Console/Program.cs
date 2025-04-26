using System.ComponentModel.DataAnnotations;
using CommandLine;
using EpochEditor.Console.CommandOptions;
using EpochEditor.SramUtilities;
using Newtonsoft.Json;

namespace EpochEditor.Console.CommandOptions {
}
internal class Program {

    private static void Main(string[] args) {

        var validProperties = typeof(ICharacterSheet).GetProperties().ToList();

        Parser
            .Default
            .ParseArguments<ReadOptions, WriteOptions>(args)
            .WithParsed<WriteOptions>(o => {
                Byte[] bytes = File.ReadAllBytes(o.FilePath);
                SramReader sr = new SramReader();
                Sram sram = sr.ReadBytes(bytes);

                if (0 > o.Slot || o.Slot >= sram.GameSlots.Length) {
                    Console.Error.WriteLine("Invalid slot index; slot index must be between 0 and {0}.", sram.GameSlots.Length - 1);
                    Environment.Exit(-1);
                }

                GameSlot gameSlot = sram.GameSlots[o.Slot];
                    
                if (0 > o.Character || o.Character >= gameSlot.CharacterSheets.Length) {
                    Console.Error.WriteLine("Invalid character index; character index must be between 0 and {0}.", gameSlot.CharacterSheets.Length - 1);
                    Environment.Exit(-1);
                }
                
                var property = validProperties.FirstOrDefault(p => p.Name.ToLower().Trim() == o.Property.ToLower().Trim());

                if (null == property) {
                    Console.Error.WriteLine("Invalid property {0}", o.Property);
                    validProperties.ForEach(p => Console.Out.WriteLine(p.Name));
                    Environment.Exit(-1);
                }

                var characterSheet = gameSlot.CharacterSheets[o.Character];
                
                Object invocationParameter;
                if (property.PropertyType == typeof(Byte)) {
                    Byte b;
                    if (false == Byte.TryParse(o.Value, out b)) {
                        Console.Error.WriteLine("Invalid one-byte integer {0}", o.Value);
                        Environment.Exit(-1);
                    }
                    invocationParameter = b;
                }
                else if (property.PropertyType == typeof(Int16)) {
                    Int16 i16;
                    if (false == Int16.TryParse(o.Value, out i16)) {
                        Console.Error.WriteLine("Invalid two-byte integer {0}", o.Value);
                        Environment.Exit(-1);
                    }
                    invocationParameter = i16;
                }
                else if (property.PropertyType == typeof(String)) {
                    invocationParameter = o.Value;
                }
                else {
                    throw new Exception();
                }

                property.GetSetMethod()?.Invoke(characterSheet, new Object[] { invocationParameter} );

                File.WriteAllBytes(o.FilePath, sram.RawBytes);
            })
            .WithParsed<ReadOptions>(o => {
                Byte[] bytes = File.ReadAllBytes(o.FilePath);
                SramReader sr = new SramReader();
                Sram sram = sr.ReadBytes(bytes);

                if (0 > o.Slot || o.Slot >= sram.GameSlots.Length) {
                    Console.Error.WriteLine("Invalid slot index; slot index must be between 0 and {0}.", sram.GameSlots.Length - 1);
                    Environment.Exit(-1);
                }

                GameSlot gameSlot = sram.GameSlots[o.Slot];

                var computedChecksum = gameSlot.ComputeChecksum();
                var loadedChecksum = gameSlot.LoadedChecksum;

                for(int slot = 0; slot < 3; slot++) {
                    if (computedChecksum != loadedChecksum) {
                        Console.Error.WriteLine("For slot {0}, computed checksum {1} does not match read checksum {2}; continuing anyway.", slot, computedChecksum, loadedChecksum);
                    }
                }

                if (o.Character != null) {
                    if (0 > o.Character || o.Character >= gameSlot.CharacterSheets.Length) {
                        Console.Error.WriteLine("Invalid character index; character index must be between 0 and {0}.", gameSlot.CharacterSheets.Length - 1);
                        Environment.Exit(-1);
                    }
                    else if (null == o.Property) {
                        Console.Out.Write(JsonConvert.SerializeObject(gameSlot.CharacterSheets[o.Character.Value], Formatting.Indented));
                        Environment.Exit(0);
                    }
                    else if (null != o.Property) {
                        var property = validProperties.FirstOrDefault(p => p.Name.ToLower().Trim() == o.Property.ToLower().Trim());

                        if (null == property) {
                            Console.Error.WriteLine("Invalid property {0}", o.Property);
                            validProperties.ForEach(p => Console.Out.WriteLine(p.Name));
                            Environment.Exit(-1);
                        }
                        else {
                            Object? propertyValue = property.GetGetMethod()?.Invoke(gameSlot.CharacterSheets[o.Character.Value], []);
                            Console.Out.WriteLine(propertyValue);
                        }
                    }
                }
                else if (null != o.Property) {
                    var property = validProperties.FirstOrDefault(p => p.Name.ToLower().Trim() == o.Property.ToLower().Trim());

                    if (null == property) {
                        Console.Error.WriteLine("Invalid property {0}", o.Property);
                        validProperties.ForEach(p => Console.Out.WriteLine(p.Name));
                        Environment.Exit(-1);
                    }
                    else {
                        List<Object?> properties = 
                            gameSlot
                                .CharacterSheets
                                .Select(cs =>  property.GetGetMethod()?.Invoke(cs, []))
                                .ToList();
                        Console.Out.WriteLine(JsonConvert.SerializeObject(properties));
                    }
                }
                else {
                    Console.Out.WriteLine(JsonConvert.SerializeObject(gameSlot.CharacterSheets, Formatting.Indented));
                }
            });
    }
}