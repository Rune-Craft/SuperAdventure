namespace Engine;

public class Vendor (string name) : INotifyPropertyChanged
{
    public string Name { get; set; } = name;
    public BindingList<InventoryItem> Inventory { get; set; } = [];

    public void AddItemToInventory (Item itemToAdd, int quantity = 1)
    {
        InventoryItem item = Inventory.SingleOrDefault(inventoryItem => inventoryItem.Details.ID == itemToAdd.ID);
        if (item == null)
        {
            // They didn't have the item, so add it to the inventory
            Inventory.Add(new InventoryItem(itemToAdd, quantity));
        }
        else
        {
            // They already had the item, so just increase the quantity
            item.Quantity += quantity;
        }
        OnPropertyChanged("Inventory");
    }

    public void RemoveItemFromInventory (Item itemToRemove, int quantity = 1)
    {
        InventoryItem item = Inventory.SingleOrDefault(inventoryItem => inventoryItem.Details.ID == itemToRemove.ID);

        if (item == null)
        {
            // They didn't have the item, so do nothing
        }
        else
        {
            // They have the item in their inventory, so decrease the quantity
            item.Quantity -= quantity;
            // Don't allow negative quantities
            if (item.Quantity < 0)
            {
                item.Quantity = 0;
            }
            // If the quantity is zero, remove the item from the inventory
            if (item.Quantity == 0)
            {
                Inventory.Remove(item);
            }
            // Notify the UI that the inventory has changed
            OnPropertyChanged("Inventory");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged (string name)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
