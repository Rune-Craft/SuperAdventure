namespace Engine;

public class Player (int currentHitPoints, int maximumHitPoints, int gold,
                    int experiencePoints) : LivingCreature
                                                      (currentHitPoints, maximumHitPoints)
{
    private int _gold;
    private int _experiencePoints;

    public int Gold
    {
        get {  return gold; }
        set
        {
            _gold = value;
            OnPropertyChanged("Gold");
        }
    }

    public int ExperiencePoints
    {
        get {  return _experiencePoints; }
        private set
        {
            _experiencePoints = value;
            OnPropertyChanged("ExperiencePoints");
            OnPropertyChanged("Level");
        }
    }

    public int Level
    {
        get { return ((ExperiencePoints / 50) + 1); }
    }

    public BindingList<InventoryItem> Inventory { get; set; } = [];

    public List<PlayerQuest> Quests { get; set; } = [];
    public Location CurrentLocation { get; set; }
    public Weapon CurrentWeapon { get; set; }

    public static Player CreateDefaultPlayer ()
    {
        Player player = new Player(10, 10, 20, 0);
        player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

        player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
        return player;
    }

    public bool HasRequiredItemToEnterThisLocation (Location location)
    {
        if (location.ItemRequiredToEnter == null)
        {
            // No item required to enter this location
            return true;
        }

        // Check if the player has the required item
        return Inventory.Any(inventoryItem => inventoryItem.Details.ID == location.ItemRequiredToEnter.ID);
    }

    public void AddExperiencePoints (int experiencePointsToAdd)
    {
        ExperiencePoints += experiencePointsToAdd;
        MaximumHitPoints = (Level * 10);
    }

    public bool HasThisQuest (Quest quest)
    {
        return Quests.Any(playerQuest => playerQuest.Details.ID == quest.ID);
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
            if (!Inventory.Any(inventoryItem => inventoryItem.Details.ID == questCompletionItem.Details.ID))
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
            InventoryItem item = Inventory.SingleOrDefault(inventoryItem =>
                                                           inventoryItem.Details.ID == questCompletionItem.Details.ID);
            if (item != null)
            {
                item.Quantity -= questCompletionItem.Quantity;
            }
        }
    }

    public void AddItemToInventory (Item itemToAdd)
    {
        InventoryItem item = Inventory.SingleOrDefault(inventoryItem =>
                                                       inventoryItem.Details.ID == itemToAdd.ID);

        if (item == null)
        {
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }
        else
        {
            item.Quantity++;
        }
    }

    public void MarkQuestCompleted (Quest quest)
    {
        PlayerQuest playerQuest = Quests.SingleOrDefault(playerQuest => playerQuest.Details.ID == quest.ID);
        if (playerQuest != null)
        {
            playerQuest.IsCompleted = true;
        }
    }

    public string ToXmlString ()
    {
        XmlDocument playerData = new XmlDocument();

        // Create or get the root element
        XmlNode player = playerData.SelectSingleNode("/Player") ?? playerData.CreateElement("Player");
        if (player.ParentNode == null) playerData.AppendChild(player);

        // Create or get the Stats node
        XmlNode stats = player.SelectSingleNode("Stats") ?? playerData.CreateElement("Stats");
        if (stats.ParentNode == null) player.AppendChild(stats);

        // Create or update the child nodes for Stats
        XmlNode currentHitPoints = stats.SelectSingleNode("CurrentHitPoints") ?? playerData.CreateElement("CurrentHitPoints");
        currentHitPoints.InnerText = this.CurrentHitPoints.ToString();
        if (currentHitPoints.ParentNode == null) stats.AppendChild(currentHitPoints);

        XmlNode maximumHitPoints = stats.SelectSingleNode("MaximumHitPoints") ?? playerData.CreateElement("MaximumHitPoints");
        maximumHitPoints.InnerText = this.MaximumHitPoints.ToString();
        if (maximumHitPoints.ParentNode == null) stats.AppendChild(maximumHitPoints);

        XmlNode gold = stats.SelectSingleNode("Gold") ?? playerData.CreateElement("Gold");
        gold.InnerText = this.Gold.ToString();
        if (gold.ParentNode == null) stats.AppendChild(gold);

        XmlNode experiencePoints = stats.SelectSingleNode("ExperiencePoints") ?? playerData.CreateElement("ExperiencePoints");
        experiencePoints.InnerText = this.ExperiencePoints.ToString();
        if (experiencePoints.ParentNode == null) stats.AppendChild(experiencePoints);

        XmlNode currentLocation = stats.SelectSingleNode("CurrentLocation") ?? playerData.CreateElement("CurrentLocation");
        currentLocation.InnerText = this.CurrentLocation.ID.ToString();
        if (currentLocation.ParentNode == null) stats.AppendChild(currentLocation);

        if (CurrentWeapon != null)
        {
            XmlNode currentWeapon = stats.SelectSingleNode("CurrentWeapon") ?? playerData.CreateElement("CurrentWeapon");
            currentWeapon.InnerText = CurrentWeapon.ID.ToString();
            if (currentWeapon.ParentNode == null) stats.AppendChild(currentWeapon);
        }

        // Create or get the Inventory Items Node
        XmlNode inventoryItems = player.SelectSingleNode("InventoryItems") ?? playerData.CreateElement("InventoryItems");
        if (inventoryItems.ParentNode == null) player.AppendChild(inventoryItems);

        // Create or update the nodes for each item in the player's inventory
        foreach (InventoryItem item in this.Inventory)
        {
            XmlNode inventoryItem = inventoryItems.SelectSingleNode($"InventoryItem[@ID='{item.Details.ID}']") ?? playerData.CreateElement("InventoryItem");

            XmlAttribute idAttribute = inventoryItem.Attributes["ID"] ?? playerData.CreateAttribute("ID");
            idAttribute.Value = item.Details.ID.ToString();
            if (inventoryItem.Attributes["ID"] == null) inventoryItem.Attributes.Append(idAttribute);

            XmlAttribute quantityAttribute = inventoryItem.Attributes["Quantity"] ?? playerData.CreateAttribute("Quantity");
            quantityAttribute.Value = item.Quantity.ToString();
            if (inventoryItem.Attributes["Quantity"] == null) inventoryItem.Attributes.Append(quantityAttribute);

            if (inventoryItem.ParentNode == null) inventoryItems.AppendChild(inventoryItem);
        }

        // Create or get the "PlayerQuests" child node
        XmlNode playerQuests = player.SelectSingleNode("PlayerQuests") ?? playerData.CreateElement("PlayerQuests");
        if (playerQuests.ParentNode == null) player.AppendChild(playerQuests);

        // Create or update a "PlayerQuest" node for each quest the player has acquired
        foreach (PlayerQuest quest in this.Quests)
        {
            XmlNode playerQuest = playerQuests.SelectSingleNode($"PlayerQuest[@ID='{quest.Details.ID}']") ?? playerData.CreateElement("PlayerQuest");

            XmlAttribute idAttribute = playerQuest.Attributes["ID"] ?? playerData.CreateAttribute("ID");
            idAttribute.Value = quest.Details.ID.ToString();
            if (playerQuest.Attributes["ID"] == null) playerQuest.Attributes.Append(idAttribute);

            XmlAttribute isCompletedAttribute = playerQuest.Attributes["IsCompleted"] ?? playerData.CreateAttribute("IsCompleted");
            isCompletedAttribute.Value = quest.IsCompleted.ToString();
            if (playerQuest.Attributes["IsCompleted"] == null) playerQuest.Attributes.Append(isCompletedAttribute);

            if (playerQuest.ParentNode == null) playerQuests.AppendChild(playerQuest);
        }

        return playerData.InnerXml; // The XML document, as a string, so we can save the data to disk
    }

    public static Player CreatePlayerFromXmlString (string xmlPlayerData)

    {
        try
        {
            XmlDocument playerData = new XmlDocument();
            playerData.LoadXml(xmlPlayerData);
            int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
            int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
            int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
            int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
            Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints);
            int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
            player.CurrentLocation = World.LocationByID(currentLocationID);

            if (playerData.SelectSingleNode("/Player/Stats/CurrentWeapon") != null)
            {
                int currentWeaponID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentWeapon").InnerText);
                player.CurrentWeapon = (Weapon)World.ItemByID(currentWeaponID);
            }

            foreach (XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
            {
                int id = Convert.ToInt32(node.Attributes["ID"].Value);
                int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);
                for (int i = 0; i < quantity; i++)
                {
                    player.AddItemToInventory(World.ItemByID(id));
                }
            }
            foreach (XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
            {
                int id = Convert.ToInt32(node.Attributes["ID"].Value);
                bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);
                PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                playerQuest.IsCompleted = isCompleted;
                player.Quests.Add(playerQuest);
            }
            return player;
        }
        catch
        {
            // If there was an error with the XML data, return a default player object
            return Player.CreateDefaultPlayer();
        }
    }
}
