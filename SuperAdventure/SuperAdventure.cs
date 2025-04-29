using Engine;

namespace SuperAdventure;

public partial class SuperAdventure : Form
{
    private Player _player;
    private Monster _currentMonster;

    public SuperAdventure ()
    {
        InitializeComponent();

        _player = new Player(10, 10, 20, 0, 1);
        MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        _player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));

        lblHitPoints.Text = _player.CurrentHitPoints.ToString();
        lblGold.Text = _player.CurrentHitPoints.ToString();
        lblExperience.Text = _player.ExperiencePoints.ToString();
        lblLevel.Text = _player.Level.ToString();
    }

    private void SuperAdventure_Load (object sender, EventArgs e)
    {
    }

    private void richTextBox2_TextChanged (object sender, EventArgs e)
    {
    }

    private void btnNorth_Click (object sender, EventArgs e)
    {
        MoveTo(_player.CurrentLocation.LocationToNorth);
    }

    private void btnEast_Click (object sender, EventArgs e)
    {
        MoveTo(_player.CurrentLocation.LocationToEast);
    }

    private void btnSouth_Click (object sender, EventArgs e)
    {
        MoveTo(_player.CurrentLocation.LocationToSouth);
    }

    private void btnWest_Click (object sender, EventArgs e)
    {
        MoveTo(_player.CurrentLocation.LocationToWest);
    }

    private void MoveTo (Location newLocation)
    {
        //Does the location have any required items
        if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
        {
            rtbMessages.Text += "You must have a " + newLocation.ItemRequiredToEnter.Name + " to enter this location." + "\n";
            return;
        }

        // Update the player's current location
        _player.CurrentLocation = newLocation;

        // Show/hide available movement buttons
        btnNorth.Visible = (newLocation.LocationToNorth != null);
        btnEast.Visible = (newLocation.LocationToEast != null);
        btnSouth.Visible = (newLocation.LocationToSouth != null);
        btnWest.Visible = (newLocation.LocationToWest != null);

        // Display current location name and description
        rtbLocation.Text = newLocation.Name + "\n";
        rtbLocation.Text += newLocation.Description + "\n";

        // Completely heal the player
        _player.CurrentHitPoints = _player.MaximumHitPoints;

        // Update Hit Points in UI
        lblHitPoints.Text = _player.CurrentHitPoints.ToString();

        // Does the location have a quest?
        if (newLocation.QuestAvailableHere != null)
        {
            // See if the player already has the quest, and if they've completed it
            bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.QuestAvailableHere);
            bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.QuestAvailableHere);

            // See if the player already has the quest
            if (playerAlreadyHasQuest)
            {
                // If the player has not completed the quest yet
                if (!playerAlreadyCompletedQuest)
                {
                    // See if the player has all the items needed to complete the quest
                    bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.QuestAvailableHere);

                    // The player has all items required to complete the quest
                    if (playerHasAllItemsToCompleteQuest)
                    {
                        // Display message
                        rtbMessages.Text += "\n";
                        rtbMessages.Text += "You complete the '" + newLocation.QuestAvailableHere.Name + "' quest." + "\n";

                        // Remove quest items from inventory
                        _player.RemoveQuestCompletionItems(newLocation.QuestAvailableHere);

                        // Give quest rewards
                        rtbMessages.Text += "You receive: " + "\n";
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardExperiencePoints.ToString() + " experience points" + "\n";
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardGold.ToString() + " gold" + "\n";
                        rtbMessages.Text += newLocation.QuestAvailableHere.RewardItem.Name + "\n";
                        rtbMessages.Text += "\n";

                        _player.ExperiencePoints += newLocation.QuestAvailableHere.RewardExperiencePoints;
                        _player.Gold += newLocation.QuestAvailableHere.RewardGold;

                        // Add the reward item to the player's inventory
                        _player.AddItemToInventory(newLocation.QuestAvailableHere.RewardItem);

                        // Mark the quest as completed
                        _player.MarkQuestCompleted(newLocation.QuestAvailableHere);
                    }
                }
            }
            else
            {
                // The player does not already have the quest

                rtbMessages.Text += "You receive the " + newLocation.QuestAvailableHere.Name + " quest." + "\n";
                rtbMessages.Text += newLocation.QuestAvailableHere.Description + "\n";
                rtbMessages.Text += "To complete it, return with:" + "\n";
                foreach (QuestCompletionItem questCompletionItem in newLocation.QuestAvailableHere.QuestCompletionItems)
                {
                    if (questCompletionItem.Quantity == 1)
                    {
                        rtbMessages.Text += questCompletionItem.Quantity.ToString() + " " + questCompletionItem.Details.Name + "\n";
                    }
                    else
                    {
                        rtbMessages.Text += questCompletionItem.Quantity.ToString() + " " + questCompletionItem.Details.NamePlural + "\n";
                    }
                }
                rtbMessages.Text += "\n";

                // Add the quest to the player's quest list
                _player.Quests.Add(new PlayerQuest(newLocation.QuestAvailableHere));
            }
        }

        // Does the location have a monster?
        if (newLocation.MonsterLivingHere != null)
        {
            rtbMessages.Text += "You see a " + newLocation.MonsterLivingHere.Name + "\n";

            // Make a new monster, using the values from the standard monster in the World.Monster list
            Monster standardMonster = World.MonsterByID(newLocation.MonsterLivingHere.ID);

            _currentMonster = new Monster(standardMonster.ID, standardMonster.Name, standardMonster.MaximumDamage,
                standardMonster.RewardExperiencePoints, standardMonster.RewardGold, standardMonster.CurrentHitPoints, standardMonster.MaximumHitPoints);

            foreach (LootItem lootItem in standardMonster.LootTable)
            {
                _currentMonster.LootTable.Add(lootItem);
            }

            cboWeapons.Visible = true;
            cboPotions.Visible = true;
            btnUseWeapon.Visible = true;
            btnUsePotion.Visible = true;
        }
        else
        {
            _currentMonster = null;

            cboWeapons.Visible = false;
            cboPotions.Visible = false;
            btnUseWeapon.Visible = false;
            btnUsePotion.Visible = false;
        }

        // Refresh player's inventory list
        UpdateInventoryListInUI();

        // Refresh player's quest list
        UpdateQuestListInUI();

        // Refresh player's weapons combobox
        UpdateWeaponListInUI();

        // Refresh player's potions combobox
        UpdatePotionListInUI();
    }

    private void UpdateInventoryListInUI ()
    {
        dgvInventory.RowHeadersVisible = false;

        dgvInventory.ColumnCount = 2;
        dgvInventory.Columns[0].Name = "Name";
        dgvInventory.Columns[0].Width = 197;
        dgvInventory.Columns[1].Name = "Quantity";

        dgvInventory.Rows.Clear();

        foreach (InventoryItem inventoryItem in _player.Inventory)
        {
            if (inventoryItem.Quantity > 0)
            {
                dgvInventory.Rows.Add(new[] { inventoryItem.Details.Name, inventoryItem.Quantity.ToString() });
            }
        }
    }

    private void UpdateQuestListInUI ()
    {
        dgvQuests.RowHeadersVisible = false;

        dgvQuests.ColumnCount = 2;
        dgvQuests.Columns[0].Name = "Name";
        dgvQuests.Columns[0].Width = 197;
        dgvQuests.Columns[1].Name = "Done?";

        dgvQuests.Rows.Clear();

        foreach (PlayerQuest playerQuest in _player.Quests)
        {
            dgvQuests.Rows.Add(new[] { playerQuest.Details.Name, playerQuest.IsCompleted.ToString() });
        }
    }

    private void UpdateWeaponListInUI ()
    {
        List<Weapon> weapons = new List<Weapon>();

        foreach (InventoryItem inventoryItem in _player.Inventory)
        {
            if (inventoryItem.Details is Weapon)
            {
                if (inventoryItem.Quantity > 0)
                {
                    weapons.Add((Weapon)inventoryItem.Details);
                }
            }
        }

        if (weapons.Count == 0)
        {
            // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
            cboWeapons.Visible = false;
            btnUseWeapon.Visible = false;
        }
        else
        {
            cboWeapons.DataSource = weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "ID";

            cboWeapons.SelectedIndex = 0;
        }
    }

    private void UpdatePotionListInUI ()
    {
        List<HealingPotion> healingPotions = new List<HealingPotion>();

        foreach (InventoryItem inventoryItem in _player.Inventory)
        {
            if (inventoryItem.Details is HealingPotion)
            {
                if (inventoryItem.Quantity > 0)
                {
                    healingPotions.Add((HealingPotion)inventoryItem.Details);
                }
            }
        }

        if (healingPotions.Count == 0)
        {
            // The player doesn't have any potions, so hide the potion combobox and "Use" button
            cboPotions.Visible = false;
            btnUsePotion.Visible = false;
        }
        else
        {
            cboPotions.DataSource = healingPotions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "ID";

            cboPotions.SelectedIndex = 0;
        }
    }

    private void btnUseWeapon_Click (object sender, EventArgs e)
    {
        // Get the currently selected weapon from cboWeapons
        Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

        int damageToMonster = RandomNumberGenerator.NumberBetween(currentWeapon.MinimumDamage, currentWeapon.MaximumDamage);

        _currentMonster.CurrentHitPoints -= damageToMonster;

        rtbMessages.Text += "You hit the " + _currentMonster.Name + " for " + damageToMonster.ToString() + " points." + "\n";

        // Check if the monster is dead
        if (_currentMonster.CurrentHitPoints <= 0)
        {
            // The monster is dead
            rtbMessages.Text += "\nYou defeated the " + _currentMonster.Name + "!" + "\n";

            // Give the player experience points
            _player.ExperiencePoints += _currentMonster.RewardExperiencePoints;
            rtbMessages.Text += "You receive " + _currentMonster.RewardExperiencePoints.ToString() + " experience points." + "\n";

            // Give the player gold
            _player.Gold += _currentMonster.RewardGold;
            rtbMessages.Text += "You receive " + _currentMonster.RewardGold.ToString() + " gold." + "\n";

            // Get random loot from the monster
            List<InventoryItem> lootedItems = new List<InventoryItem>();

            // Add items to the lootedItems list, comparing a random number to drop the percentage
            foreach (LootItem lootItem in _currentMonster.LootTable)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.DropPercentage)
                {
                    lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                }
            }

            // If no items were randomly selected, then add the default loot item(s)
            if (lootedItems.Count == 0)
            {
                foreach (LootItem lootItem in _currentMonster.LootTable)
                {
                    if (lootItem.IsDefaultItem)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.Details, 1));
                    }
                }
            }

            // Add the looted items to the player's inventory
            foreach (InventoryItem inventoryItem in lootedItems)
            {
                _player.AddItemToInventory(inventoryItem.Details);

                if (inventoryItem.Quantity == 1)
                {
                    rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.Name + "\n";
                }
                else
                {
                    rtbMessages.Text += "You loot " + inventoryItem.Quantity.ToString() + " " + inventoryItem.Details.NamePlural + "\n";
                }
            }

            // Refresh the player information and inventory controls
            lblHitPoints.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();

            UpdateInventoryListInUI();
            UpdateWeaponListInUI();
            UpdatePotionListInUI();

            rtbMessages.Text += "\n";

            // Move player to current location (to heal player and create a new monster to fight)
            MoveTo(_player.CurrentLocation);
        }
        else
        {
            // Monster is still alive

            // Determine the amount of damage the monster does to the player
            int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

            // Display the message
            rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() +
                                " points of damage.\n";

            _player.CurrentHitPoints -= damageToPlayer;

            lblHitPoints.Text = _player.CurrentHitPoints.ToString();

            if (_player.CurrentHitPoints <= 0)
            {
                // Display message
                rtbMessages.Text += "The " + _currentMonster.Name + " killed you!" + "\n";

                // Move the player to "Home"
                MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            }
        }
    }

    private void btnUsePotion_Click (object sender, EventArgs e)
    {
        // Get the currently selected potion
        HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

        // Add healing amount to the player's current hit points
        _player.CurrentHitPoints = (_player.CurrentHitPoints + potion.AmountToHeal);

        // CurrentHitPoints cannot exceed MaximumHitPoints
        if (_player.CurrentHitPoints > _player.MaximumHitPoints)
        {
            _player.CurrentHitPoints = _player.MaximumHitPoints;
        }

        // Remove the potion from the player's inventory
        foreach (InventoryItem inventoryItem in _player.Inventory)
        {
            if (inventoryItem.Details.ID == potion.ID)
            {
                inventoryItem.Quantity--;
                break;
            }
        }

        rtbMessages.Text += "You drink a " + potion.Name + "\n";

        // Monster Attacks back

        // Determine the amount of damage the monster does to the player
        int damageToPlayer = RandomNumberGenerator.NumberBetween(0, _currentMonster.MaximumDamage);

        rtbMessages.Text += "The " + _currentMonster.Name + " did " + damageToPlayer.ToString() +
                            " points of damage.\n";

        _player.CurrentHitPoints -= damageToPlayer;

        if (_player.CurrentHitPoints <= 0)
        {
            rtbMessages.Text += "The " + _currentMonster.Name + " killed you!" + "\n";

            // Move the player to "Home"
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
        }

        // Refresh the player information and inventory controls
        lblHitPoints.Text = _player.CurrentHitPoints.ToString();
        UpdateInventoryListInUI();
        UpdatePotionListInUI();
    }
}
