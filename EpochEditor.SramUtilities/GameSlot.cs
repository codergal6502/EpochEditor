using System.Reflection;

namespace EpochEditor.SramUtilities;

public class GameSlot {
    private const int SIZE_OF_16_BITS = sizeof(UInt16); // It's just 2, but I find magic numbers confusing.
    private const int SIZE_OF_32_BITS = sizeof(UInt32); // It's just 4, but I find magic numbers confusing.
    private readonly Byte[] _rawBytes;
    private readonly int _slotIndex;
    private readonly int _slotOffset;

    public GameSlot(Byte[] rawBytes, int slotIndex, int slotOffset) {
        this._rawBytes = rawBytes;
        this._slotIndex = slotIndex;
        this.CharacterSheets = new ICharacterSheet[8];
        this.Inventory = new InventoryItem[SramConstants.INVENTORY_SIZE];
        this._slotOffset = slotOffset;
    }

    public ICharacterSheet[] CharacterSheets { get; }
    public InventoryItem[] Inventory { get; }

    // Through experimenting with a hex editor, the following values in 0x580 result in these parties:
    //
    // 00 80 80: Crono
    // 00 01 80: Crono  Marle
    // 00 02 80: Crono  Lucca
    // 00 03 80: Crono  R66-Y
    // 80 05 06: Ayla   Magus
    // 04 05 06: Frog   Ayla   Magus
    // 06 05 04: Magus  Ayla   Frog
    // 
    // I'm assuming then that 0x80 means "nobody" and that 0x00-0x06 just mean characters 1-6.

    private Byte? GetPartyMember(int partyMemberOffset) {
        Byte characterIndex = this._rawBytes[this._slotOffset + partyMemberOffset];
        if (characterIndex > SramConstants.MAX_CHARACTER_INDEX) {
            return null;
        }
        else {
            return characterIndex;
        }
    }

    private void SetPartyMember(int partyMemberOffset, Byte? charcterIndex) {
        Byte byteToWrite = SramConstants.EMPTY_PARTY_SLOT;
        if (null != charcterIndex) {
            if (0 <= charcterIndex && charcterIndex <= SramConstants.MAX_CHARACTER_INDEX) {
                byteToWrite = charcterIndex.Value;
            }
        }

        this._rawBytes[this._slotOffset + partyMemberOffset] = byteToWrite;
    }

    public Byte? PartyMemberOne   {
        get {
            return GetPartyMember(SramConstants.PARTY_MEMBER_ONE_OFFSET);
        }
        set { 
            SetPartyMember(SramConstants.PARTY_MEMBER_ONE_OFFSET, value);
            UpdateChecksumBytes();
        } 
    }

    public Byte? PartyMemberTwo   {
        get {
            return GetPartyMember(SramConstants.PARTY_MEMBER_TWO_OFFSET);
        }
        set { 
            SetPartyMember(SramConstants.PARTY_MEMBER_TWO_OFFSET, value);
            UpdateChecksumBytes();
        } 
    }

    public Byte? PartyMemberThree   {
        get {
            return GetPartyMember(SramConstants.PARTY_MEMBER_THREE_OFFSET);
        }
        set { 
            SetPartyMember(SramConstants.PARTY_MEMBER_THREE_OFFSET, value);
            UpdateChecksumBytes();
        } 
    }

    public Byte? SaveCount {
        get {
            return this._rawBytes[this._slotOffset + SramConstants.SAVE_COUNT_OFFSET];
        }
        set {
            this._rawBytes[this._slotOffset + SramConstants.SAVE_COUNT_OFFSET] = value ?? SramConstants.MINIMUM_SAVE_COUNT;
            UpdateChecksumBytes();
        }
    }

    public Byte? PlayerX {
        get {
            return this._rawBytes[this._slotOffset + SramConstants.PLAYER_X_OFFSET];
        }
        set {
            this._rawBytes[this._slotOffset + SramConstants.PLAYER_X_OFFSET] = value ?? default;
            UpdateChecksumBytes();
        }
    }

    public Byte? PlayerY {
        get {
            return this._rawBytes[this._slotOffset + SramConstants.PLAYER_Y_OFFSET];
        }
        set {
            this._rawBytes[this._slotOffset + SramConstants.PLAYER_Y_OFFSET] = value ?? default;
            UpdateChecksumBytes();
        }
    }

    public UInt32? Gold {
        get {
            var sramGoldBytes = this._rawBytes.AsSpan(this._slotOffset + SramConstants.GOLD_OFFSET, SramConstants.GOLD_LENGTH);
            Byte[] valueIntBytes = new Byte[SIZE_OF_32_BITS];
            sramGoldBytes.CopyTo(valueIntBytes);
            return BitConverter.ToUInt32(valueIntBytes);
        }
        set {
            UInt32 valueToStore;

            if (null == value) valueToStore = 0;
            else if (value > SramConstants.GOLD_MAX) valueToStore = SramConstants.GOLD_MAX;
            else valueToStore = value.Value;

            Byte[] tooManyBytes = new Byte[SIZE_OF_32_BITS];
            BitConverter.TryWriteBytes(tooManyBytes, valueToStore);
            var justRightBytes = tooManyBytes.AsSpan(0, SramConstants.GOLD_LENGTH);
            var sramGoldBytes = this._rawBytes.AsSpan(this._slotOffset + SramConstants.GOLD_OFFSET, SramConstants.GOLD_LENGTH);
            
            justRightBytes.CopyTo(sramGoldBytes);

            UpdateChecksumBytes();
        }
    }
    
    public UInt16? World { 
        get {
            return BitConverter.ToUInt16(_rawBytes, SramConstants.WORLD_OFFSET);
        }
        set {
            if (null != value) {
                BitConverter.TryWriteBytes(new Span<byte>(_rawBytes, SramConstants.WORLD_OFFSET, SIZE_OF_16_BITS), value.Value);
                UpdateChecksumBytes();
            }
        }
    }

    public void UpdateBytesFromCharacterSheets()
    {
        Span<Byte> slotSpan = new Span<byte>(_rawBytes, this._slotOffset, SramConstants.SLOT_LENGTH);

        int currentCharacterOffset = SramConstants.FIRST_CHARACTER_OFFSET;
        for (int i = 0; i < CharacterSheets.Length; i++)
        {
            ICharacterSheet characterSheet = CharacterSheets[i];

            foreach (PropertyInfo prop in typeof(ICharacterSheet).GetProperties())
            {
                CharacterSheetPropertyAttribute? attr = Attribute.GetCustomAttribute(prop, typeof(CharacterSheetPropertyAttribute)) as CharacterSheetPropertyAttribute;
                if (null != attr)
                {
                    int pos = currentCharacterOffset + attr.Offset;

                    if (typeof(Byte) == prop.PropertyType)
                    {
                        Byte? i8 = (Byte?)prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i8.HasValue) { throw new Exception(); }

                        slotSpan[pos] = i8.Value;
                    }
                    else if (typeof(Int16) == prop.PropertyType)
                    {
                        Int16? i16 = (Int16?)prop.GetGetMethod()?.Invoke(characterSheet, []);

                        if (false == i16.HasValue) { throw new Exception(); }

                        BitConverter.TryWriteBytes(slotSpan.Slice(pos, 2), i16.Value);
                    }
                }
            }

            Char[] nameChars = characterSheet.Name.Take(6).ToArray();
            Byte[] nameBytes = nameChars.Select(c => SramConstants.ASCII_TO_CT_CHAR[c]).ToArray();
            int nameLocation = 0x5B0 + characterSheet.NameId * 6;

            var nameSpan = slotSpan.Slice(nameLocation, SramConstants.NAME_LENGTH);
            new Byte[] { 0, 0, 0, 0, 0, 0 }.CopyTo(nameSpan);
            nameBytes.CopyTo(nameSpan);

            currentCharacterOffset += SramConstants.CHARACTER_SHEET_LENGTH;
        }

        UpdateChecksumBytes();
    }

    public void UpdateBytesFromItemId(int itemSlotNumber)
    {
        if (0 > itemSlotNumber || itemSlotNumber > SramConstants.INVENTORY_SIZE) {
            throw new Exception($"Item slot number {itemSlotNumber} not between 0 and {SramConstants.INVENTORY_SIZE}.");
        }

        // The inventory item IDs literally start at byte 0 in the slot.
        _rawBytes[_slotOffset + itemSlotNumber] = this.Inventory[itemSlotNumber].ItemId;
        
        UpdateChecksumBytes();
    }

    public void UpdateBytesFromItemCount(int itemSlotNumber)
    {
        if (0 > itemSlotNumber || itemSlotNumber > SramConstants.INVENTORY_SIZE) {
            throw new Exception($"Item slot number {itemSlotNumber} not between 0 and {SramConstants.INVENTORY_SIZE}.");
        }

        _rawBytes[_slotOffset + SramConstants.INVENTORY_COUNT_START_OFFSET + itemSlotNumber] = this.Inventory[itemSlotNumber].ItemCount;
        
        UpdateChecksumBytes();
    }

    private void UpdateChecksumBytes()
    {
        var checksumPosition = 0x1FF0 + _slotIndex * 2;

        Span<byte> checksumSpan = new(_rawBytes, checksumPosition, 2);
        BitConverter.TryWriteBytes(new Span<byte>(_rawBytes, checksumPosition, 2), ComputeChecksum());
    }
    
    public UInt16? LoadedChecksum { get; set; }

    public UInt16 ComputeChecksum() {
        UInt32 checksum = 0;

        // See https://github.com/mcred/chrono-trigger-save-editor/blob/master/internal/app/Editor.go#L80.
        // See https://github.com/mikearnos/snessum/blob/master/snessum.c#L145

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
