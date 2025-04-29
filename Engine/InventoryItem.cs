namespace Engine;

public class InventoryItem (Item details, int quantity)
{
    public Item Details { get; set; } = details;
    public int Quantity { get; set; } = quantity;
}