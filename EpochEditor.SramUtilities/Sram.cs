using System.Reflection;

namespace EpochEditor.SramUtilities;

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
                CharacterSheetPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterSheetPropertyAttribute)) as CharacterSheetPropertyAttribute;
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
    
        var newChecksums = ComputeChecksums();
        for (int slot = 0; slot < 3; slot++) {
            var checksumPosition = 0x1FF0 + slot * 2;
            
            Span<byte> checksumSpan = new(RawBytes, checksumPosition, 2);
            BitConverter.TryWriteBytes(new Span<byte>(RawBytes, checksumPosition, 2), newChecksums[slot]);
        }
    }

    public UInt16[] ComputeChecksums() {
        UInt16[] checksums = [0, 0, 0];

        for (int slot = 0; slot < 3; slot++) {
            UInt32 checksum = 0;

            for (int i = 0x9FE; i >= 0; i -= 2) {
                if (checksum > 0xFFFF) {
                    checksum -= 0xFFFF;
                }

                var position = i + slot * 0xA00;
                Span<byte> span = new(RawBytes, position, 2);
                UInt16 twoBytes = BitConverter.ToUInt16(span);

                checksum += twoBytes;
            }
            checksum &= 0XFFFF;

            checksums[slot] = Convert.ToUInt16(checksum);
        }
        
        return checksums;
    }

    public UInt16[] ReadChecksums() {
        UInt16[] checksums = [0, 0, 0];
        for (int slot = 0; slot < 3; slot++) {
            var checksumPosition = 0x1FF0 + slot * 2;
            Span<byte> checksumSpan = new(RawBytes, checksumPosition, 2);
            checksums[slot] = BitConverter.ToUInt16(checksumSpan);
        }

        return checksums;
    }
}
