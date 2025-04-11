namespace EpochEditor.SramUtilities;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
internal class CharacterCardPropertyAttribute : Attribute {
    public CharacterCardPropertyAttribute(int offset) {
        Offset = offset;
    }
    public int Offset { get; set; }
}
