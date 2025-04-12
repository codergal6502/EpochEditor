namespace EpochEditor.SramUtilities;

public interface ICharacterSheet
{
    string Name { get; set; }
    [CharacterCardProperty(0x00)]
    public Byte NameId { get; set; }
    [CharacterCardProperty(0x01)]
    public Byte CharId { get; set; }
    [CharacterCardProperty(0x03)]
    public Int16 HitPoints { get; set; }
    [CharacterCardProperty(0x05)]
    public Int16 MaxHitPoints { get; set; }
    [CharacterCardProperty(0x07)]
    public Int16 MagicPoints { get; set; }
    [CharacterCardProperty(0x09)]
    public Int16 MaxMagicPoints { get; set; }
    [CharacterCardProperty(0x0B)]
    public Byte BasePower { get; set; }
    [CharacterCardProperty(0x0C)]
    public Byte BaseStamina { get; set; }
    [CharacterCardProperty(0x0D)]
    public Byte BaseSpeed { get; set; }
    [CharacterCardProperty(0x0E)]
    public Byte BaseMagic { get; set; }
    [CharacterCardProperty(0x0F)]
    public Byte BaseHit { get; set; }
    [CharacterCardProperty(0x10)]
    public Byte BaseEvade { get; set; }
    [CharacterCardProperty(0x11)]
    public Byte BaseMagicDefense { get; set; }
    [CharacterCardProperty(0x12)]
    public Byte Level { get; set; }
    [CharacterCardProperty(0x27)]
    public Byte Helmet { get; set; }
    [CharacterCardProperty(0x28)]
    public Byte Armor { get; set; }
    [CharacterCardProperty(0x29)]
    public Byte Weapon { get; set; }
    [CharacterCardProperty(0x2A)]
    public Byte Relic { get; set; }
    [CharacterCardProperty(0x2B)]
    public Int16 ExpToLevel { get; set; }
    [CharacterCardProperty(0x36)]
    public Byte CurrentPower { get; set; }
    [CharacterCardProperty(0x37)]
    public Byte CurrentStamina { get; set; }
    [CharacterCardProperty(0x38)]
    public Byte CurrentSpeed { get; set; }
    [CharacterCardProperty(0x39)]
    public Byte CurrentMagic { get; set; }
    [CharacterCardProperty(0x3A)]
    public Byte CurrentHit { get; set; }
    [CharacterCardProperty(0x3B)]
    public Byte CurrentEvade { get; set; }
    [CharacterCardProperty(0x3C)]
    public Byte CurrentMagicDefense { get; set; }
    [CharacterCardProperty(0x3D)]
    public Byte CurrentAttack { get; set; }
    [CharacterCardProperty(0x3E)]
    public Byte CurrentDefense { get; set; }
    [CharacterCardProperty(0x3F)]
    public Int16 CurrentMaxHitPoints { get; set; }
}
