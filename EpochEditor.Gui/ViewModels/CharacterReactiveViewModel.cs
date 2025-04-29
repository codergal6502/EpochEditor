using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using DynamicData.Binding;
using EpochEditor.SramUtilities;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class CharacterReactiveViewModel : ReactiveObject {
    private bool _shouldBeVisible;

    public CharacterReactiveViewModel(SramReactiveViewModel sramViewModel, int editorGroupIndex, ICharacterSheet sramCharacterSheet, bool shouldBeVisible) {
        this.SramViewModel = sramViewModel;
        this.EditorGroupIndex = editorGroupIndex;
        this.SramCharacterSheet = sramCharacterSheet;
        this.ShouldBeVisible = shouldBeVisible;
    }

    public SramReactiveViewModel SramViewModel { get; }

    public int EditorGroupIndex { get; }

    public ICharacterSheet SramCharacterSheet { get; }

    public bool ShouldBeVisible { get => _shouldBeVisible; set => this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }

    public string Name {
        get {
            return SramCharacterSheet.Name;
        }
        set {
            if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Name != value) {
                SramCharacterSheet.Name = value;
                CopyNameToCharactersWithSameNameIndex();                
                SramViewModel.RaiseSlotChecksumChange();
                this.RaisePropertyChanged();
            }
        }
    }

    private void CopyNameToCharactersWithSameNameIndex() {
        foreach (var otherCharacterSheet in this.SramViewModel.Characters) {
            if (Object.ReferenceEquals(this, otherCharacterSheet)) {
                continue;
            }
            else {
                if (otherCharacterSheet.NameId == this.NameId) {
                    otherCharacterSheet.Name = this.Name;
                }
            }
        }
    }

    public byte     NameId              { get { return SramCharacterSheet.NameId;              } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.NameId != value)              { SramCharacterSheet.NameId = value;              SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CharId              { get { return SramCharacterSheet.CharId;              } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CharId != value)              { SramCharacterSheet.CharId = value;              SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    HitPoints           { get { return SramCharacterSheet.HitPoints;           } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.HitPoints != value)           { SramCharacterSheet.HitPoints = value;           SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    MaxHitPoints        { get { return SramCharacterSheet.MaxHitPoints;        } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.MaxHitPoints != value)        { SramCharacterSheet.MaxHitPoints = value;        SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    MagicPoints         { get { return SramCharacterSheet.MagicPoints;         } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.MagicPoints != value)         { SramCharacterSheet.MagicPoints = value;         SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    MaxMagicPoints      { get { return SramCharacterSheet.MaxMagicPoints;      } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.MaxMagicPoints != value)      { SramCharacterSheet.MaxMagicPoints = value;      SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BasePower           { get { return SramCharacterSheet.BasePower;           } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BasePower != value)           { SramCharacterSheet.BasePower = value;           SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseStamina         { get { return SramCharacterSheet.BaseStamina;         } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseStamina != value)         { SramCharacterSheet.BaseStamina = value;         SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseSpeed           { get { return SramCharacterSheet.BaseSpeed;           } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseSpeed != value)           { SramCharacterSheet.BaseSpeed = value;           SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseMagic           { get { return SramCharacterSheet.BaseMagic;           } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseMagic != value)           { SramCharacterSheet.BaseMagic = value;           SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseHit             { get { return SramCharacterSheet.BaseHit;             } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseHit != value)             { SramCharacterSheet.BaseHit = value;             SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseEvade           { get { return SramCharacterSheet.BaseEvade;           } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseEvade != value)           { SramCharacterSheet.BaseEvade = value;           SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     BaseMagicDefense    { get { return SramCharacterSheet.BaseMagicDefense;    } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.BaseMagicDefense != value)    { SramCharacterSheet.BaseMagicDefense = value;    SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Level               { get { return SramCharacterSheet.Level;               } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Level != value)               { SramCharacterSheet.Level = value;               SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Helmet              { get { return SramCharacterSheet.Helmet;              } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Helmet != value)              { SramCharacterSheet.Helmet = value;              SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Armor               { get { return SramCharacterSheet.Armor;               } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Armor != value)               { SramCharacterSheet.Armor = value;               SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Weapon              { get { return SramCharacterSheet.Weapon;              } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Weapon != value)              { SramCharacterSheet.Weapon = value;              SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Relic               { get { return SramCharacterSheet.Relic;               } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.Relic != value)               { SramCharacterSheet.Relic = value;               SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    ExpToLevel          { get { return SramCharacterSheet.ExpToLevel;          } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.ExpToLevel != value)          { SramCharacterSheet.ExpToLevel = value;          SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentPower        { get { return SramCharacterSheet.CurrentPower;        } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentPower != value)        { SramCharacterSheet.CurrentPower = value;        SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentStamina      { get { return SramCharacterSheet.CurrentStamina;      } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentStamina != value)      { SramCharacterSheet.CurrentStamina = value;      SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentSpeed        { get { return SramCharacterSheet.CurrentSpeed;        } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentSpeed != value)        { SramCharacterSheet.CurrentSpeed = value;        SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentMagic        { get { return SramCharacterSheet.CurrentMagic;        } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentMagic != value)        { SramCharacterSheet.CurrentMagic = value;        SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentHit          { get { return SramCharacterSheet.CurrentHit;          } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentHit != value)          { SramCharacterSheet.CurrentHit = value;          SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentEvade        { get { return SramCharacterSheet.CurrentEvade;        } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentEvade != value)        { SramCharacterSheet.CurrentEvade = value;        SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentMagicDefense { get { return SramCharacterSheet.CurrentMagicDefense; } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentMagicDefense != value) { SramCharacterSheet.CurrentMagicDefense = value; SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentAttack       { get { return SramCharacterSheet.CurrentAttack;       } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentAttack != value)       { SramCharacterSheet.CurrentAttack = value;       SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentDefense      { get { return SramCharacterSheet.CurrentDefense;      } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentDefense != value)      { SramCharacterSheet.CurrentDefense = value;      SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    CurrentMaxHitPoints { get { return SramCharacterSheet.CurrentMaxHitPoints; } set { if (false == SramViewModel.SramIsLoading && SramCharacterSheet.CurrentMaxHitPoints != value) { SramCharacterSheet.CurrentMaxHitPoints = value; SramViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
}