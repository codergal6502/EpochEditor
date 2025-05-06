namespace EpochEditor.SramUtilities;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
internal class CharacterSheetPropertyAttribute : Attribute {
    public CharacterSheetPropertyAttribute(int offset) {
        Offset = offset;
    }
    public int Offset { get; set; }
}
