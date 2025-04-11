using System.ComponentModel.DataAnnotations;
using CommandLine;
using EpochEditor.Console.CommandOptions;
using EpochEditor.SramUtilities;
using Newtonsoft.Json;

namespace EpochEditor.Console.CommandOptions {
}
internal class Program {

    private static void Main(string[] args) {

        var validProperties = typeof(CharacterSheet).GetProperties().ToList();

        Parser
            .Default
            .ParseArguments<ReadOptions, WriteOptions>(args)
            .WithParsed<ReadOptions>(o => {
                Byte[] bytes = File.ReadAllBytes(o.FilePath);
                SramReader sr = new SramReader();
                Sram sram = sr.ReadBytes(bytes);

                if (o.Character != null) {
                    if (0 > o.Character || o.Character >= sram.CharacterSheets.Length) {
                        Console.Error.WriteLine("Invalid character index; character index must be between 0 and {0}.", sram.CharacterSheets.Length - 1);
                        Environment.Exit(-1);
                    }
                    else if (null == o.Property) {
                        Console.Out.Write(JsonConvert.SerializeObject(sram.CharacterSheets[o.Character.Value], Formatting.Indented));
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
                            Object? propertyValue = property.GetGetMethod()?.Invoke(sram.CharacterSheets[o.Character.Value], []);
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
                            sram
                                .CharacterSheets
                                .Select(cs =>  property.GetGetMethod()?.Invoke(cs, []))
                                .ToList();
                        Console.Out.WriteLine(JsonConvert.SerializeObject(properties));
                    }
                }
                else {
                    Console.Out.WriteLine(JsonConvert.SerializeObject(sram.CharacterSheets, Formatting.Indented));
                }
            })
            .WithNotParsed(errors => {

            });
    }
}