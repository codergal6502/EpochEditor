namespace EpochEditor.SramUtilities;

public class CharacterSheet : ICharacterSheet
{
    public String Name { get; set; } = String.Empty;

    public Byte NameId { get; set; }

    public Byte CharId { get; set; }

    public Int16 HitPoints { get; set; }

    public Int16 MaxHitPoints { get; set; }


    public Int16 MagicPoints { get; set; }

    public Int16 MaxMagicPoints { get; set; }

    public Byte BasePower { get; set; }

    public Byte BaseStamina { get; set; }

    public Byte BaseSpeed { get; set; }

    public Byte BaseMagic { get; set; }

    public Byte BaseHit { get; set; }

    public Byte BaseEvade { get; set; }

    public Byte BaseMagicDefense { get; set; }

    public Byte Level { get; set; }

    public Byte Helmet { get; set; }

    public Byte Armor { get; set; }

    public Byte Weapon { get; set; }

    public Byte Relic { get; set; }

    public Int16 ExpToLevel { get; set; }

    public Byte CurrentPower { get; set; }

    public Byte CurrentStamina { get; set; }

    public Byte CurrentSpeed { get; set; }

    public Byte CurrentMagic { get; set; }

    public Byte CurrentHit { get; set; }

    public Byte CurrentEvade { get; set; }

    public Byte CurrentMagicDefense { get; set; }

    public Byte CurrentAttack { get; set; }

    public Byte CurrentDefense { get; set; }

    public Int16 CurrentMaxHitPoints { get; set; }
}
