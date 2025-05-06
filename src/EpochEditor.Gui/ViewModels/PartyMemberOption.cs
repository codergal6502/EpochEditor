using System;

namespace EpochEditor.Gui.ViewModels;

public class PartyMemberOption
{
    public Byte? Index { get; set; } = null;
    public String Name { get; set; } = String.Empty;
    public String DisplayName { get => null == Index ? "<empty>" : String.IsNullOrWhiteSpace(Name) ? $"Character {Index}" : $"Character {Index} ({Name})"; }
}