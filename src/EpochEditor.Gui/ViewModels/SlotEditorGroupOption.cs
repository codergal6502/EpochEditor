using System.Collections.Generic;
using System.Linq;

namespace EpochEditor.Gui.ViewModels;

public class SlotEditorGroupOption : EditorGroupOption {
    public enum SlotEditorGroupType { Inventory, GameState };

    public required SlotEditorGroupType GroupType { get; set; }

    public override string DisplayName => GroupType.ToString();

    internal static IEnumerable<SlotEditorGroupType> GetAllTypes()
    {
        return typeof(SlotEditorGroupType).GetEnumValues().OfType<SlotEditorGroupType>();
    }
}