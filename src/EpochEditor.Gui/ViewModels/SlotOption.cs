namespace EpochEditor.Gui.ViewModels;

public class SlotOption {
    public int SlotIndex { get; set; }
    public int SlotNumber { get => SlotIndex + 1; }

    public string DisplayName => $"Slot {SlotNumber}";
}