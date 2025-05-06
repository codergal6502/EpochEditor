namespace EpochEditor.SramUtilities;

public static class ItemList {
    public static List<Item> AllItems { get; }

    static ItemList() {
        using (var stream = typeof(ItemList).Assembly.GetManifestResourceStream("EpochEditor.SramUtilities.ItemList.json"))
        using (var reader = new StreamReader(stream ?? throw new Exception("ItemList.json stream was null."))) {
            AllItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Item>>(reader.ReadToEnd()) ?? throw new Exception("ItemList.json parsed to null.");
        }
    }

    public enum ItemType { 
        Katana, Bow, Gun, Arm, Broadsword, Fist, Scythe, Armor, Helmet, Accessory
    }
    public class Item {
        public Byte ItemId { get; set; } = 0x00;
        public String ItemName { get; set; } = String.Empty;
        public ItemType? ItemType { get; set; } = null;
    }
}