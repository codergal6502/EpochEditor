// See https://datacrystal.tcrf.net/wiki/Chrono_Trigger_(SNES)/RAM_map

using System.Reflection.Metadata;

namespace EpochEditor.SramUtilities;

public class SramConstants {
    public const int SRAM_FILE_LENGTH = 0x2000;
    public const int SLOT_COUNT = 3;
    public const int SLOT_LENGTH = 0x0A00;
    public const int SLOT_1_START = 0x0000;
    public const int SLOT_2_START = 0x0A00;
    public const int SLOT_3_START = 0x1400;
    public const int CHECKSUM_OFFSET = 0x1FF0;
    public const int NAME_LENGTH = 6;
    public const int FIRST_CHARACTER_OFFSET = 0x200;
    public const int CHARACTER_SHEET_LENGTH = 0x050;
    public const int INVENTORY_COUNT_START_OFFSET = 0x100;
    public const int INVENTORY_SIZE = 0x100;
    public const int LAST_SAVE_SLOT_INDEX_LOCATION = 0x1FE0;
    public const int PARTY_MEMBER_ONE_OFFSET = 0x580;
    public const int PARTY_MEMBER_TWO_OFFSET = 0x581;
    public const int PARTY_MEMBER_THREE_OFFSET = 0x582;
    public const int MAX_CHARACTER_INDEX = 6;
    public const int EMPTY_PARTY_SLOT = 0x80;
    public const int SAVE_COUNT_OFFSET = 0x59C;
    public const byte MINIMUM_SAVE_COUNT = 1;
    public const int GOLD_OFFSET = 0x5E0;
    public const int GOLD_LENGTH = 3;
    public const UInt32 GOLD_MAX = 9999999;
    public const int WORLD_OFFSET = 0x5F3;
    public const int PLAYER_X_OFFSET = 0x5F5;
    public const int PLAYER_Y_OFFSET = 0x5F6;

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
