using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using DynamicData.Binding;
using EpochEditor.SramUtilities;
using ReactiveUI;
using static EpochEditor.Gui.ViewModels.InventoryViewModel;

namespace EpochEditor.Gui.ViewModels;

public class CharacterViewModel : ReactiveObject {
    private bool _shouldBeVisible;
    private GameSlotViewModel _gameSlotViewModel;

    public CharacterViewModel(GameSlotViewModel sramViewModel, int editorGroupIndex, ICharacterSheet sramCharacterSheet, bool shouldBeVisible) {
        this._gameSlotViewModel = sramViewModel;
        this.EditorGroupIndex = editorGroupIndex;
        this.SramCharacterSheet = sramCharacterSheet;
        this.ShouldBeVisible = shouldBeVisible;
        GameItemOptions = ItemList.AllItems.Select(i => new GameItem { ItemId = i.ItemId, Name = i.ItemName, ItemType = i.ItemType }).ToList();
    }

    public int EditorGroupIndex { get; }

    public ICharacterSheet SramCharacterSheet { get; }

    public bool ShouldBeVisible { get => _shouldBeVisible; set => this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }

    public List<GameItem> GameItemOptions { get; }

    public string Name {
        get {
            return SramCharacterSheet.Name;
        }
        set {
            if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Name != value) {
                SramCharacterSheet.Name = value;
                CopyNameToCharactersWithSameNameIndex();                
                _gameSlotViewModel.RaiseSlotChecksumChange();
                this.RaisePropertyChanged();
            }
        }
    }

    private void CopyNameToCharactersWithSameNameIndex() {
        foreach (var otherCharacterSheet in this._gameSlotViewModel.CharacterViewModels) {
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

    public byte     NameId              { get { return SramCharacterSheet.NameId;              } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.NameId != value)              { SramCharacterSheet.NameId           = value;      _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CharId              { get { return SramCharacterSheet.CharId;              } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CharId != value)              { SramCharacterSheet.CharId           = value;      _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short?   HitPoints           { get { return SramCharacterSheet.HitPoints;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.HitPoints != value)           { SramCharacterSheet.HitPoints        = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short?   MaxHitPoints        { get { return SramCharacterSheet.MaxHitPoints;        } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.MaxHitPoints != value)        { SramCharacterSheet.MaxHitPoints     = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short?   MagicPoints         { get { return SramCharacterSheet.MagicPoints;         } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.MagicPoints != value)         { SramCharacterSheet.MagicPoints      = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short?   MaxMagicPoints      { get { return SramCharacterSheet.MaxMagicPoints;      } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.MaxMagicPoints != value)      { SramCharacterSheet.MaxMagicPoints   = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BasePower           { get { return SramCharacterSheet.BasePower;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BasePower != value)           { SramCharacterSheet.BasePower        = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseStamina         { get { return SramCharacterSheet.BaseStamina;         } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseStamina != value)         { SramCharacterSheet.BaseStamina      = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseSpeed           { get { return SramCharacterSheet.BaseSpeed;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseSpeed != value)           { SramCharacterSheet.BaseSpeed        = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseMagic           { get { return SramCharacterSheet.BaseMagic;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseMagic != value)           { SramCharacterSheet.BaseMagic        = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseHit             { get { return SramCharacterSheet.BaseHit;             } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseHit != value)             { SramCharacterSheet.BaseHit          = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseEvade           { get { return SramCharacterSheet.BaseEvade;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseEvade != value)           { SramCharacterSheet.BaseEvade        = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte?    BaseMagicDefense    { get { return SramCharacterSheet.BaseMagicDefense;    } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.BaseMagicDefense != value)    { SramCharacterSheet.BaseMagicDefense = value ?? 0; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Level               { get { return SramCharacterSheet.Level;               } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Level != value)               { SramCharacterSheet.Level = value;               _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     Helmet              { get { return SramCharacterSheet.Helmet;              } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Helmet != value)              { SramCharacterSheet.Helmet = value;              _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(HelmetIndex));     } } }
    public byte     Armor               { get { return SramCharacterSheet.Armor;               } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Armor != value)               { SramCharacterSheet.Armor = value;               _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(ArmorIndex));      } } }
    public byte     Weapon              { get { return SramCharacterSheet.Weapon;              } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Weapon != value)              { SramCharacterSheet.Weapon = value;              _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(WeaponIndex));     } } }
    public byte     Accessory           { get { return SramCharacterSheet.Accessory;           } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.Accessory != value)           { SramCharacterSheet.Accessory = value;           _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); this.RaisePropertyChanged(nameof(AccessoryIndex));  } } }
    public short    ExpToLevel          { get { return SramCharacterSheet.ExpToLevel;          } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.ExpToLevel != value)          { SramCharacterSheet.ExpToLevel = value;          _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentPower        { get { return SramCharacterSheet.CurrentPower;        } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentPower != value)        { SramCharacterSheet.CurrentPower = value;        _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentStamina      { get { return SramCharacterSheet.CurrentStamina;      } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentStamina != value)      { SramCharacterSheet.CurrentStamina = value;      _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentSpeed        { get { return SramCharacterSheet.CurrentSpeed;        } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentSpeed != value)        { SramCharacterSheet.CurrentSpeed = value;        _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentMagic        { get { return SramCharacterSheet.CurrentMagic;        } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentMagic != value)        { SramCharacterSheet.CurrentMagic = value;        _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentHit          { get { return SramCharacterSheet.CurrentHit;          } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentHit != value)          { SramCharacterSheet.CurrentHit = value;          _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentEvade        { get { return SramCharacterSheet.CurrentEvade;        } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentEvade != value)        { SramCharacterSheet.CurrentEvade = value;        _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentMagicDefense { get { return SramCharacterSheet.CurrentMagicDefense; } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentMagicDefense != value) { SramCharacterSheet.CurrentMagicDefense = value; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentAttack       { get { return SramCharacterSheet.CurrentAttack;       } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentAttack != value)       { SramCharacterSheet.CurrentAttack = value;       _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public byte     CurrentDefense      { get { return SramCharacterSheet.CurrentDefense;      } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentDefense != value)      { SramCharacterSheet.CurrentDefense = value;      _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }
    public short    CurrentMaxHitPoints { get { return SramCharacterSheet.CurrentMaxHitPoints; } set { if (false == _gameSlotViewModel.IsSramLoading() && SramCharacterSheet.CurrentMaxHitPoints != value) { SramCharacterSheet.CurrentMaxHitPoints = value; _gameSlotViewModel.RaiseSlotChecksumChange(); this.RaisePropertyChanged(); } } }


    private int ItemIdToGetItemOptionsIndex(byte itemId)
    {
        var selectedItem = GameItemOptions.FirstOrDefault(gio => gio.ItemId == itemId);
        return null == selectedItem ? 0 : GameItemOptions.IndexOf(selectedItem);
    }

    private byte GetItemOptionsIndexToItemId(int itemIndex)
    {
        var selectedItem = GameItemOptions.Skip(itemIndex).FirstOrDefault();
        var byteId = selectedItem?.ItemId ?? 0;
        return byteId;
    }

    public int HelmetIndex     { get { Byte itemId = Helmet;     return ItemIdToGetItemOptionsIndex(itemId); } set { byte byteId = GetItemOptionsIndexToItemId(value); Helmet    = byteId; } }
    public int ArmorIndex      { get { Byte itemId = Armor;      return ItemIdToGetItemOptionsIndex(itemId); } set { byte byteId = GetItemOptionsIndexToItemId(value); Armor     = byteId; } }
    public int WeaponIndex     { get { Byte itemId = Weapon;     return ItemIdToGetItemOptionsIndex(itemId); } set { byte byteId = GetItemOptionsIndexToItemId(value); Weapon    = byteId; } }
    public int AccessoryIndex  { get { Byte itemId = Accessory;  return ItemIdToGetItemOptionsIndex(itemId); } set { byte byteId = GetItemOptionsIndexToItemId(value); Accessory = byteId; } }
}