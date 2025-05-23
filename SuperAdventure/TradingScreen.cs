﻿namespace SuperAdventure;

public partial class TradingScreen : Form
{
    private Player _currentPlayer;
    //public Player CurrentPlayer { get; set; }

    public TradingScreen (Player player)
    {
        _currentPlayer = player;
        InitializeComponent();

        // Style, to display numeric column values
        DataGridViewCellStyle rightAlignedCellStyle = new DataGridViewCellStyle();
        rightAlignedCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        // Populate the datagrid view with the vendor's inventory
        dgvMyItems.RowHeadersVisible = false;
        dgvMyItems.AutoGenerateColumns = false;

        // This hidden column holds the item ID, so we know which item to sell
        dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemID",
            Visible = false
        });

        dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Name",
            Width = 100,
            DataPropertyName = "Description"
        });

        dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Qty",
            Width = 30,
            DefaultCellStyle = rightAlignedCellStyle,
            DataPropertyName = "Price",
        });

        dgvMyItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Price",
            Width = 35,
            DefaultCellStyle = rightAlignedCellStyle,
            DataPropertyName = "Price"
        });

        dgvMyItems.Columns.Add(new DataGridViewButtonColumn
        {
            Text = "Sell 1",
            UseColumnTextForButtonValue = true,
            Width = 50,
            DataPropertyName = "ItemID"
        });

        // Bind the player's inventory to the datagridview
        dgvMyItems.DataSource = _currentPlayer.Inventory;
        // When the user clicks on a row, call this function
        dgvMyItems.CellClick += dgvMyItems_CellClick;

        // Populate the datagrid for the vendor's inventory
        dgvVendorItems.RowHeadersVisible = false;
        dgvVendorItems.AutoGenerateColumns = false;

        // This hidden column holds the item ID, so we know which item to sell
        dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ItemID",
            Visible = false
        });

        dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Name",
            Width = 100,
            DataPropertyName = "Description"
        });

        dgvVendorItems.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Price",
            Width = 35,
            DefaultCellStyle = rightAlignedCellStyle,
            DataPropertyName = "Price"
        });

        dgvVendorItems.Columns.Add(new DataGridViewButtonColumn
        {
            Text = "Buy 1",
            UseColumnTextForButtonValue = true,
            Width = 50,
            DataPropertyName = "ItemID"
        });

        //Bind the vendor's inventory to the datagridview
        dgvVendorItems.DataSource = _currentPlayer.CurrentLocation.VendorWorkingHere.Inventory;
        // When the user clicks on a row, call this function
        dgvVendorItems.CellClick += dgvVendorItems_CellClick;
    }

    private void dgvMyItems_CellClick (object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == 4)
        {
            var itemID = dgvMyItems.Rows[e.RowIndex].Cells[0].Value;

            Item itemBeingSold = World.ItemByID(Convert.ToInt32(itemID));
            if (itemBeingSold.Price == World.UNSELLABLE_ITEM_PRICE)
            {
                MessageBox.Show("You cannot sell the " + itemBeingSold.Name);
            }
            else
            {
                _currentPlayer.RemoveItemFromInventory(itemBeingSold);
                _currentPlayer.Gold += itemBeingSold.Price;
            }
        }
    }

    private void dgvVendorItems_CellClick (object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == 3)
        {
            var itemID = dgvVendorItems.Rows[e.RowIndex].Cells[0].Value;
            Item itemBeingBought = World.ItemByID(Convert.ToInt32(itemID));

            if (_currentPlayer.Gold >= itemBeingBought.Price)
            {
                _currentPlayer.AddItemToInventory(itemBeingBought);

                _currentPlayer.Gold -= itemBeingBought.Price;
            }
            else
            {
                MessageBox.Show("You do not have enough gold to buy the " + itemBeingBought.Name);
            }
        }
    }

    private void btnClose_Click (object sender, EventArgs e)
    {
        Close();
    }
}
