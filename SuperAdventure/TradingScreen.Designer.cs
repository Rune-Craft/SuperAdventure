namespace SuperAdventure
{
    partial class TradingScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            lblMyInventory = new Label();
            lblVendorInventory = new Label();
            dgvMyItems = new DataGridView();
            dgvVendorItems = new DataGridView();
            btnClose = new Button();
            ((ISupportInitialize)dgvMyItems).BeginInit();
            ((ISupportInitialize)dgvVendorItems).BeginInit();
            SuspendLayout();
            // 
            // lblMyInventory
            // 
            lblMyInventory.AutoSize = true;
            lblMyInventory.Location = new Point(99, 13);
            lblMyInventory.Name = "lblMyInventory";
            lblMyInventory.Size = new Size(77, 15);
            lblMyInventory.TabIndex = 0;
            lblMyInventory.Text = "My Inventory";
            // 
            // lblVendorInventory
            // 
            lblVendorInventory.AutoSize = true;
            lblVendorInventory.Location = new Point(349, 13);
            lblVendorInventory.Name = "lblVendorInventory";
            lblVendorInventory.Size = new Size(105, 15);
            lblVendorInventory.TabIndex = 1;
            lblVendorInventory.Text = "Vendor's Inventory";
            // 
            // dgvMyItems
            // 
            dgvMyItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMyItems.Location = new Point(13, 43);
            dgvMyItems.Name = "dgvMyItems";
            dgvMyItems.Size = new Size(240, 216);
            dgvMyItems.TabIndex = 2;
            // 
            // dgvVendorItems
            // 
            dgvVendorItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVendorItems.Location = new Point(276, 43);
            dgvVendorItems.Name = "dgvVendorItems";
            dgvVendorItems.Size = new Size(240, 216);
            dgvVendorItems.TabIndex = 3;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(441, 274);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 23);
            btnClose.TabIndex = 4;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // TradingScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 310);
            Controls.Add(btnClose);
            Controls.Add(dgvVendorItems);
            Controls.Add(dgvMyItems);
            Controls.Add(lblVendorInventory);
            Controls.Add(lblMyInventory);
            Name = "TradingScreen";
            Text = "TradingScreen";
            ((ISupportInitialize)dgvMyItems).EndInit();
            ((ISupportInitialize)dgvVendorItems).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblMyInventory;
        private Label lblVendorInventory;
        private DataGridView dgvMyItems;
        private DataGridView dgvVendorItems;
        private Button btnClose;
    }
}