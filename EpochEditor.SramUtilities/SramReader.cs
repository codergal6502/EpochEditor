using System.Collections;
using System.Reflection;

namespace EpochEditor.SramUtilities;

public class SramReader
{
    private const int CHARACTER_COUNT = 7;

    private static readonly Dictionary<Byte, Char> CT_CHAR_TO_ASCII;

    static SramReader() {
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
    }

    public SramReader(Byte[] bytes) {

        const int marleStart = 0x250;

        CharacterSheet marleCard = new CharacterSheet();

        foreach(PropertyInfo prop in typeof(CharacterSheet).GetProperties()) {
            CharacterCardPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterCardPropertyAttribute)) as CharacterCardPropertyAttribute;
            if (null != attr) {
                int pos = marleStart + attr.Offset;

                if (typeof(Byte) == prop.PropertyType) {
                    Byte i8 = bytes[pos];
                    prop.GetSetMethod()?.Invoke(marleCard, new object[] { i8 });
                }
                else if (typeof(Int16) == prop.PropertyType) {
                    Int16 i16 = BitConverter.ToInt16(bytes, pos);
                    prop.GetSetMethod()?.Invoke(marleCard, new object[] { i16 });
                }
            }
        }

        marleCard.Name = String.Concat(bytes.Skip(0x5B0 + marleCard.NameId * 6).Take(6).TakeWhile(b => b != 0x00).Select(b => CT_CHAR_TO_ASCII[b]));
    }
}
