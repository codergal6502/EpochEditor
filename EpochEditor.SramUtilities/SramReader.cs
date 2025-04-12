using System.Collections;
using System.Reflection;

namespace EpochEditor.SramUtilities;

internal class SramConstants {
    public const int FIRST_CHARACTER_OFFSET = 0x200;
    public const int CHARACTER_SHEET_LENGTH = 0x050;
    public static readonly Dictionary<Byte, Char> CT_CHAR_TO_ASCII;
    public static readonly Dictionary<Char, Byte> ASCII_TO_CT_CHAR;

    static SramConstants() {
        CT_CHAR_TO_ASCII = new Dictionary<Byte, Char> {
            { 0xA0, 'A' },
            { 0xA1, 'B' },
            { 0xA2, 'C' },
            { 0xA3, 'D' },
            { 0xA4, 'E' },
            { 0xA5, 'F' },
            { 0xA6, 'G' },
            { 0xA7, 'H' },
            { 0xA8, 'I' },
            { 0xA9, 'J' },
            { 0xAA, 'K' },
            { 0xAB, 'L' },
            { 0xAC, 'M' },
            { 0xAD, 'N' },
            { 0xAE, 'O' },
            { 0xAF, 'P' },
            { 0xB0, 'Q' },
            { 0xB1, 'R' },
            { 0xB2, 'S' },
            { 0xB3, 'T' },
            { 0xB4, 'U' },
            { 0xB5, 'V' },
            { 0xB6, 'W' },
            { 0xB7, 'X' },
            { 0xB8, 'Y' },
            { 0xB9, 'Z' },
            { 0xBA, 'a' },
            { 0xBB, 'b' },
            { 0xBC, 'c' },
            { 0xBD, 'd' },
            { 0xBE, 'e' },
            { 0xBF, 'f' },
            { 0xC0, 'g' },
            { 0xC1, 'h' },
            { 0xC2, 'i' },
            { 0xC3, 'j' },
            { 0xC4, 'k' },
            { 0xC5, 'l' },
            { 0xC6, 'm' },
            { 0xC7, 'n' },
            { 0xC8, 'o' },
            { 0xC9, 'p' },
            { 0xCA, 'q' },
            { 0xCB, 'r' },
            { 0xCC, 's' },
            { 0xCD, 't' },
            { 0xCE, 'u' },
            { 0xCF, 'v' },
            { 0xD0, 'w' },
            { 0xD1, 'x' },
            { 0xD2, 'y' },
            { 0xD3, 'z' },
            { 0xD4, '0' },
            { 0xD5, '1' },
            { 0xD6, '2' },
            { 0xD7, '3' },
            { 0xD8, '4' },
            { 0xD9, '5' },
            { 0xDA, '6' },
            { 0xDB, '7' },
            { 0xDC, '8' },
            { 0xDD, '9' },
            { 0xDE, '!' },
            { 0xDF, '?' },
            { 0xE0, '/' },
            { 0xE1, '“' },
            { 0xE2, '”' },
            { 0xE3, ':' },
            { 0xE4, '%' },
            { 0xE5, '(' },
            { 0xE6, ')' },
            { 0xE7, '\'' },
            { 0xE8, '.' },
            { 0xE9, ',' },
            { 0xEA, '=' },
            { 0xEB, '-' },
            { 0xEC, '+' },
            { 0xED, '\\' },
            { 0xFF, ' ' },
        };
        ASCII_TO_CT_CHAR = new Dictionary<Char, Byte>(CT_CHAR_TO_ASCII.Select(kvp => KeyValuePair.Create(kvp.Value, kvp.Key)));
    }
}

public class Sram {
    public Sram() {
        this.RawBytes = new byte[0x2000];
        this.CharacterSheets = new ICharacterSheet[8];
    }
    
    public Byte[] RawBytes { get; private set; }
    public ICharacterSheet[] CharacterSheets { get; private set; }

    public void UpdateBytesFromCharacterSheets()
    {
        int currentCharacterOffset = SramConstants.FIRST_CHARACTER_OFFSET;
        for (int i = 0; i < CharacterSheets.Length; i++) {
            ICharacterSheet characterSheet = CharacterSheets[i];

            foreach(PropertyInfo prop in typeof(ICharacterSheet).GetProperties()) {
                CharacterCardPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterCardPropertyAttribute)) as CharacterCardPropertyAttribute;
                if (null != attr) {
                    int pos = currentCharacterOffset + attr.Offset;

                    if (typeof(Byte) == prop.PropertyType) {
                        Byte? i8 = (Byte?) prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i8.HasValue) { throw new Exception(); }

                        RawBytes[pos] = i8.Value;
                    }
                    else if (typeof(Int16) == prop.PropertyType) {
                        Int16? i16 = (Int16?) prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i16.HasValue) { throw new Exception(); }

                        BitConverter.TryWriteBytes(new Span<byte>(RawBytes, pos, 2), i16.Value);
                    }
                }
            }

            Char[] nameChars = characterSheet.Name.Take(6).ToArray();
            Byte[] nameBytes = nameChars.Select(c => SramConstants.ASCII_TO_CT_CHAR[c]).ToArray();
            int nameLocation = 0x5B0 + characterSheet.NameId * 6;

            Array.Copy(new Byte[] {0, 0, 0, 0, 0, 0}, 0, RawBytes, nameLocation, 6);
            Array.Copy(nameBytes, 0, RawBytes, nameLocation, Math.Min(nameBytes.Length, 6));
            
            currentCharacterOffset += SramConstants.CHARACTER_SHEET_LENGTH;
        }
    }
}

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
