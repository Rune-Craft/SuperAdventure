namespace Engine;

public class QuestCompletionItem (Item details, int quantity)
{
    public Item Details { get; set; } = details;
    public int Quantity { get; set; } = quantity;
}