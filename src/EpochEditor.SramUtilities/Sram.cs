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

    public Byte LastSaveSlotIndex { get => this.RawBytes[SramConstants.LAST_SAVE_SLOT_INDEX_LOCATION]; set => this.RawBytes[SramConstants.LAST_SAVE_SLOT_INDEX_LOCATION] = value; }

    public void UpdateBytes() {
        for(var slotIndex = 0; slotIndex < GameSlots.Length; slotIndex++) {
            var gameSlot = GameSlots[slotIndex];
            gameSlot.UpdateBytesFromCharacterSheets();
        }
    }
}
