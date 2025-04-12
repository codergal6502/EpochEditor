using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace EpochEditor.SramUtilities;

public class SramReader
{
    public SramReader() { }

    public Sram ReadBytes(Byte[] bytes) {
        Sram sram = new Sram();

        if (bytes.Length != sram.RawBytes.Length) {
            throw new Exception($"bad length {bytes.Length}");
        }

        Buffer.BlockCopy(bytes, 0, sram.RawBytes, 0, sram.RawBytes.Length);

        int currentCharacterOffset = SramConstants.FIRST_CHARACTER_OFFSET;
        for (int i = 0; i < sram.CharacterSheets.Length; i++) {
            CharacterSheet characterSheet = new CharacterSheet();

            foreach(PropertyInfo prop in typeof(ICharacterSheet).GetProperties()) {
                CharacterCardPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterCardPropertyAttribute)) as CharacterCardPropertyAttribute;
                if (null != attr) {
                    int pos = currentCharacterOffset + attr.Offset;

                    if (typeof(Byte) == prop.PropertyType) {
                        Byte i8 = bytes[pos];
                        prop.GetSetMethod()?.Invoke(characterSheet, [i8]);
                    }
                    else if (typeof(Int16) == prop.PropertyType) {
                        Int16 i16 = BitConverter.ToInt16(bytes, pos);
                        prop.GetSetMethod()?.Invoke(characterSheet, [i16]);
                    }
                }
            }

            characterSheet.Name = String.Concat(bytes.Skip(0x5B0 + characterSheet.NameId * 6).Take(6).TakeWhile(b => b != 0x00).Select(b => SramConstants.CT_CHAR_TO_ASCII[b]));
            sram.CharacterSheets[i] = CharacterSheetProxy.CreateProxy(characterSheet, sram);
            currentCharacterOffset += SramConstants.CHARACTER_SHEET_LENGTH;
        }

        return sram;
    }
}
