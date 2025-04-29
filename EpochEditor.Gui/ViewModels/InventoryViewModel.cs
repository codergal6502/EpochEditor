using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using EpochEditor.SramUtilities;
using ReactiveUI;

namespace EpochEditor.Gui.ViewModels;

public class InventoryViewModel : ReactiveObject
{
    private bool _shouldBeVisible = false;
    private SramReactiveViewModel _sramViewModel;
    private SramUtilities.InventoryItem[] _inventory;

    public InventoryViewModel(SramReactiveViewModel sramReactiveViewModel, SramUtilities.InventoryItem[] inventory)
    {
        this._sramViewModel = sramReactiveViewModel;
        this._inventory = inventory;

        GameItemOptions = ItemList.AllItems.Select(i => new GameItem { ItemId = i.ItemId, Name = i.ItemName, ItemType = i.ItemType }).ToList();
        InventoryItems = inventory.Select((item, itemSlot) => new InventoryItem(_sramViewModel, item, _inventory, GameItemOptions, itemSlot)).ToList();

        this.RaisePropertyChanged(nameof(InventoryItems));
    }

    public List<GameItem> GameItemOptions { get; }

    public class GameItem { 
        public Byte ItemId { get; set; } = 0x00;
        public String Name { get; set; } = String.Empty;
        public ItemList.ItemType? ItemType { get; set; } = null;
        public String DisplayName { get => ItemType == null ? $"0x{ItemId:X2}: {Name}" : $"0x{ItemId:X2}: {Name} ({ItemType})"; }
    }

    public bool ShouldBeVisible { get { return _shouldBeVisible; } set { this.RaiseAndSetIfChanged(ref _shouldBeVisible, value); }}
    
    public List<InventoryItem> InventoryItems { get; set; }

    public class InventoryItem : ReactiveObject {
        private readonly SramReactiveViewModel _sramViewModel;
        private readonly SramUtilities.InventoryItem _sramItem;
        private readonly SramUtilities.InventoryItem[] _inventory;
        private readonly int _itemSlot;

        public InventoryItem(SramReactiveViewModel sramViewModel, SramUtilities.InventoryItem sramItem, SramUtilities.InventoryItem[] inventory, List<GameItem> gameItems, int itemSlot)
        {
            this._sramViewModel = sramViewModel;
            this._sramItem = sramItem;
            this._inventory = inventory;
            this._itemSlot = itemSlot;

            GameItemOptions = gameItems;
        }

        public int ItemSlot { get => _itemSlot; }

        public int GameItemIndex {
            get {
                var itemId = _inventory[_itemSlot].ItemId;
                var gameItemOption = GameItemOptions.Where(gio => gio.ItemId == itemId).Single();
                return GameItemOptions.IndexOf(gameItemOption);
            }
            set { 
                if (false == _sramViewModel.SramIsLoading) {
                    var gameItemOption = GameItemOptions[value];
                    SramUtilities.InventoryItem sramInventoryItem = _inventory[_itemSlot];

                    if (gameItemOption.ItemId != sramInventoryItem.ItemId) {
                        sramInventoryItem.ItemId = gameItemOption.ItemId;
                        _sramViewModel.RaiseSlotChecksumChange();
                        this.RaisePropertyChanged();

                        if (0x00 == gameItemOption.ItemId) {
                            ItemCount = 0;
                        }
                        else if (ItemCount == 0) {
                            ItemCount = 1;
                        }
                    }
                }
            }
        }
        public Byte ItemCount {
            get { return _inventory[_itemSlot].ItemCount; }
            set { 
                if (false == _sramViewModel.SramIsLoading && _inventory[_itemSlot].ItemCount != value ) {
                    _inventory[_itemSlot].ItemCount = value;
                    _sramViewModel.RaiseSlotChecksumChange();
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(ShouldItemCountBeEnabled));
                }
            }
        }

        public Boolean ShouldItemCountBeEnabled { get { return ItemCount > 0; }}

        public List<GameItem> GameItemOptions { get; }
    }
}