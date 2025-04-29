namespace EpochEditor.SramUtilities;

public class InventoryItem
{
    private byte _itemId;
    private byte _itemCount;
    private GameSlot _gameSlot;

    public InventoryItem(GameSlot gameSlot, Byte itemId, Byte itemCount) {
        this._gameSlot = gameSlot;
        this._itemId = itemId;
        this._itemCount = itemCount;
    }

    public Byte ItemId {
        get => _itemId;
        set {
            _itemId = value;
            var itemSlotNumber = Array.IndexOf(_gameSlot.Inventory, this);
            _gameSlot.UpdateBytesFromItemId(itemSlotNumber);
        }
    }

    public Byte ItemCount {
        get => _itemCount;
        set {
            _itemCount = value;
            var itemSlotNumber = Array.IndexOf(_gameSlot.Inventory, this);
            _gameSlot.UpdateBytesFromItemCount(itemSlotNumber);
        }
    }
}