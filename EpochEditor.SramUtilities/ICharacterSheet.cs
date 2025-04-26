namespace EpochEditor.SramUtilities;

public interface ICharacterSheet
{
    string Name { get; set; }
    [CharacterSheetProperty(0x00)]
    public Byte NameId { get; set; }
    [CharacterSheetProperty(0x01)]
    public Byte CharId { get; set; }
    [CharacterSheetProperty(0x03)]
    public Int16 HitPoints { get; set; }
    [CharacterSheetProperty(0x05)]
    public Int16 MaxHitPoints { get; set; }
    [CharacterSheetProperty(0x07)]
    public Int16 MagicPoints { get; set; }
    [CharacterSheetProperty(0x09)]
    public Int16 MaxMagicPoints { get; set; }
    [CharacterSheetProperty(0x0B)]
    public Byte BasePower { get; set; }
    [CharacterSheetProperty(0x0C)]
    public Byte BaseStamina { get; set; }
    [CharacterSheetProperty(0x0D)]
    public Byte BaseSpeed { get; set; }
    [CharacterSheetProperty(0x0E)]
    public Byte BaseMagic { get; set; }
    [CharacterSheetProperty(0x0F)]
    public Byte BaseHit { get; set; }
    [CharacterSheetProperty(0x10)]
    public Byte BaseEvade { get; set; }
    [CharacterSheetProperty(0x11)]
    public Byte BaseMagicDefense { get; set; }
    [CharacterSheetProperty(0x12)]
    public Byte Level { get; set; }
    [CharacterSheetProperty(0x27)]
    public Byte Helmet { get; set; }
    [CharacterSheetProperty(0x28)]
    public Byte Armor { get; set; }
    [CharacterSheetProperty(0x29)]
    public Byte Weapon { get; set; }
    [CharacterSheetProperty(0x2A)]
    public Byte Relic { get; set; }
    [CharacterSheetProperty(0x2B)]
    public Int16 ExpToLevel { get; set; }
    [CharacterSheetProperty(0x36)]
    public Byte CurrentPower { get; set; }
    [CharacterSheetProperty(0x37)]
    public Byte CurrentStamina { get; set; }
    [CharacterSheetProperty(0x38)]
    public Byte CurrentSpeed { get; set; }
    [CharacterSheetProperty(0x39)]
    public Byte CurrentMagic { get; set; }
    [CharacterSheetProperty(0x3A)]
    public Byte CurrentHit { get; set; }
    [CharacterSheetProperty(0x3B)]
    public Byte CurrentEvade { get; set; }
    [CharacterSheetProperty(0x3C)]
    public Byte CurrentMagicDefense { get; set; }
    [CharacterSheetProperty(0x3D)]
    public Byte CurrentAttack { get; set; }
    [CharacterSheetProperty(0x3E)]
    public Byte CurrentDefense { get; set; }
    [CharacterSheetProperty(0x3F)]
    public Int16 CurrentMaxHitPoints { get; set; }
}
