using System.Reflection;

namespace EpochEditor.SramUtilities;

public class Sram {
    public Sram() {
        // See https://datacrystal.tcrf.net/wiki/Chrono_Trigger_(SNES)/RAM_map.

        this.RawBytes = new byte[0x2000];
        this.GameSlots = [new GameSlot(RawBytes, 0, SramConstants.SLOT_1_START), new GameSlot(RawBytes, 1, SramConstants.SLOT_2_START), new GameSlot(RawBytes, 2, SramConstants.SLOT_3_START)];
    }
    
    public Byte[] RawBytes { get; }

    public Byte LastSave { get; set; }

    public GameSlot[] GameSlots { get; }

    public void UpdateBytes() {
        for(var slotIndex = 0; slotIndex < GameSlots.Length; slotIndex++) {
            var gameSlot = GameSlots[slotIndex];
            gameSlot.UpdateBytesFromCharacterSheets();
        }
    }
}

public class GameSlot {
    private readonly Byte[] _rawBytes;
    private readonly int _slotIndex;
    private readonly int _slotOffset;

    public GameSlot(Byte[] rawBytes, int slotIndex, int slotOffset) {
        this._rawBytes = rawBytes;
        this._slotIndex = slotIndex;
        this.CharacterSheets = new ICharacterSheet[8];
        this._slotOffset = slotOffset;
    }

    public ICharacterSheet[] CharacterSheets { get; }



    public void UpdateBytesFromCharacterSheets()
    {
        Span<Byte> slotSpan = new Span<byte>(_rawBytes, this._slotOffset, SramConstants.SLOT_LENGTH);
        
        int currentCharacterOffset = SramConstants.FIRST_CHARACTER_OFFSET;
        for (int i = 0; i < CharacterSheets.Length; i++) {
            ICharacterSheet characterSheet = CharacterSheets[i];

            foreach(PropertyInfo prop in typeof(ICharacterSheet).GetProperties()) {
                CharacterSheetPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterSheetPropertyAttribute)) as CharacterSheetPropertyAttribute;
                if (null != attr) {
                    int pos = currentCharacterOffset + attr.Offset;

                    if (typeof(Byte) == prop.PropertyType) {
                        Byte? i8 = (Byte?) prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i8.HasValue) { throw new Exception(); }

                        slotSpan[pos] = i8.Value;
                    }
                    else if (typeof(Int16) == prop.PropertyType) {
                        Int16? i16 = (Int16?) prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i16.HasValue) { throw new Exception(); }

                        BitConverter.TryWriteBytes(slotSpan.Slice(pos, 2), i16.Value);
                    }
                }
            }

            Char[] nameChars = characterSheet.Name.Take(6).ToArray();
            Byte[] nameBytes = nameChars.Select(c => SramConstants.ASCII_TO_CT_CHAR[c]).ToArray();
            int nameLocation = 0x5B0 + characterSheet.NameId * 6;

            var nameSpan = slotSpan.Slice(nameLocation, SramConstants.NAME_LENGTH);
            new Byte[] {0, 0, 0, 0, 0, 0}.CopyTo(nameSpan);
            nameBytes.CopyTo(nameSpan);
            
            currentCharacterOffset += SramConstants.CHARACTER_SHEET_LENGTH;
        }

        var checksumPosition = 0x1FF0 + _slotIndex * 2;
        
        Span<byte> checksumSpan = new(_rawBytes, checksumPosition, 2);
        BitConverter.TryWriteBytes(new Span<byte>(_rawBytes, checksumPosition, 2), ComputeChecksum());
    }

    public UInt16? LoadedChecksum { get; set; }

    public UInt16 ComputeChecksum() {
        UInt32 checksum = 0;

        for (int i = 0x9FE; i >= 0; i -= 2) {
            if (checksum > 0xFFFF) {
                checksum -= 0xFFFF;
            }

            var position = i + _slotIndex * 0xA00;
            Span<byte> span = new Span<Byte>(_rawBytes, position, 2);
            UInt16 twoBytes = BitConverter.ToUInt16(span);

            checksum += twoBytes;
        }
        checksum &= 0XFFFF;

        return Convert.ToUInt16(checksum);
    }
}
