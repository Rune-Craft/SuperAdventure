using System.Runtime.InteropServices.Marshalling;

namespace Engine;

public class LootItem (Item details, int dropPercentage, bool isDefaultItem)
{
    public Item Details { get; set; } = details;
    public int DropPercentage { get; set; } = dropPercentage;
    public bool IsDefaultItem { get; set; } = isDefaultItem;
}