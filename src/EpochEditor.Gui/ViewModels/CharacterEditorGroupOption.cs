using System;

namespace EpochEditor.Gui.ViewModels;

public class CharacterEditorGroupOption : EditorGroupOption {
    public int CharacterIndex { get; set; }

    public String? CharacterName { get; set; }

    public override string DisplayName => String.IsNullOrWhiteSpace(CharacterName) ? $"Character {CharacterIndex}" : $"Character {CharacterIndex} ({CharacterName})";
}
