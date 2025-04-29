using System;

namespace EpochEditor.Gui.ViewModels;

public abstract class EditorGroupOption {
    public abstract String DisplayName { get; }
}

public class CharacterEditorGroupOption : EditorGroupOption {
    public int CharacterIndex { get; set; }

    public String? CharacterName { get; set; }

    public override string DisplayName => String.IsNullOrWhiteSpace(CharacterName) ? $"Character {CharacterIndex}" : $"Character {CharacterIndex} ({CharacterName})";
}

// This isn't yet supported; comment this back in when it is.
// public class SlotEditorGroupOption : EditorGroupOption {
//     public required String Group { get; set; }

//     public override string DisplayName => Group;
// }