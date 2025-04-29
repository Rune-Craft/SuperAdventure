namespace Engine;

public class Player (int currentHitPoints, int maximumHitPoints, int gold,
                    int experiencePoints, int level) : LivingCreature
                                                      (currentHitPoints, maximumHitPoints)
{
    public int Gold { get; set; } = gold;

    public int ExperiencePoints { get; set; } = experiencePoints;
    public int Level { get; set; } = level;
    public List<InventoryItem> Inventory { get; set; } = [];
    public List<PlayerQuest> Quests { get; set; } = [];
    public Location CurrentLocation { get; set; }

    public bool HasRequiredItemToEnterThisLocation (Location location)
    {
        if (location.ItemRequiredToEnter == null)
        {
            // No item required to enter this location
            return true;
        }

        // Check if the player has the required item
        foreach (InventoryItem inventoryItem in Inventory)
        {
            if (inventoryItem.Details.ID == location.ItemRequiredToEnter.ID)
            {
                return true;
            }
        }

        // The player does not have the required item
        return false;
    }

    public bool HasThisQuest (Quest quest)
    {
        foreach (PlayerQuest playerQuest in Quests)
        {
            if (playerQuest.Details.ID == quest.ID)
            {
                return true;
            }
        }

        return false;
    }

    public bool CompletedThisQuest (Quest quest)
    {
        foreach (PlayerQuest playerQuest in Quests)
        {
            if (playerQuest.Details.ID == quest.ID)
            {
                return playerQuest.IsCompleted;
            }
        }

        return false;
    }

    public bool HasAllQuestCompletionItems (Quest quest)
    {
        // Check if the player has all the required items to complete the quest
        foreach (QuestCompletionItem questCompletionItem in quest.QuestCompletionItems)
        {
            bool foundItemInPlayersInventory = false;

            // Check each item in the player's inventory, to see if they have it, and enough
            foreach (InventoryItem inventoryItem in Inventory)
            {
                if (inventoryItem.Details.ID == questCompletionItem.Details.ID)
                {
                    foundItemInPlayersInventory = true;

                    if (inventoryItem.Quantity < questCompletionItem.Quantity)
                    {
                        return false;
                    }
                }
            }

            // The player does not have the required item
            if (!foundItemInPlayersInventory)
            {
                return false;
            }
        }

        // The player has all the required items
        return true;
    }

    public void RemoveQuestCompletionItems (Quest quest)
    {
        foreach (QuestCompletionItem questCompletionItem in quest.QuestCompletionItems)
        {
            foreach (InventoryItem inventoryItem in Inventory)
            {
                if (inventoryItem.Details.ID == questCompletionItem.Details.ID)
                {
                    // Remove the item from the player's inventory
                    inventoryItem.Quantity -= questCompletionItem.Quantity;
                    break;
                }
            }
        }
    }

    public void AddItemToInventory (Item itemToAdd)
    {
        foreach (InventoryItem inventoryItem in Inventory)
        {
            if (inventoryItem.Details.ID == itemToAdd.ID)
            {
                // The item is already in the inventory, so just increase the quantity
                inventoryItem.Quantity++;

                return;
            }
        }

        // The item is not in the inventory, so add it
        Inventory.Add(new InventoryItem(itemToAdd, 1));
    }

    public void MarkQuestCompleted (Quest quest)
    {
        // Find the quest in the player's list of quests
        foreach (PlayerQuest playerQuest in Quests)
        {
            if (playerQuest.Details.ID == quest.ID)
            {
                // Mark it as completed
                playerQuest.IsCompleted = true;

                return;
            }
        }
    }
}
