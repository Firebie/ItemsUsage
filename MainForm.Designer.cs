namespace ItemsUsage
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
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
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this._btnDelete = new System.Windows.Forms.Button();
      this._btnEdit = new System.Windows.Forms.Button();
      this._btnAdd = new System.Windows.Forms.Button();
      this._gvItems = new System.Windows.Forms.DataGridView();
      this.objectBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
      this._mainMenu = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tooslToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.carsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.inventoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this._btnExit = new System.Windows.Forms.Button();
      this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.carNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).BeginInit();
      this._mainMenu.SuspendLayout();
      this.SuspendLayout();
      // 
      // _btnDelete
      // 
      this._btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnDelete.Location = new System.Drawing.Point(216, 344);
      this._btnDelete.Name = "_btnDelete";
      this._btnDelete.Size = new System.Drawing.Size(96, 23);
      this._btnDelete.TabIndex = 9;
      this._btnDelete.Text = "Удалить";
      this._btnDelete.UseVisualStyleBackColor = true;
      this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
      // 
      // _btnEdit
      // 
      this._btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnEdit.Location = new System.Drawing.Point(114, 344);
      this._btnEdit.Name = "_btnEdit";
      this._btnEdit.Size = new System.Drawing.Size(96, 23);
      this._btnEdit.TabIndex = 8;
      this._btnEdit.Text = "Редактировать";
      this._btnEdit.UseVisualStyleBackColor = true;
      this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
      // 
      // _btnAdd
      // 
      this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnAdd.Location = new System.Drawing.Point(12, 344);
      this._btnAdd.Name = "_btnAdd";
      this._btnAdd.Size = new System.Drawing.Size(96, 23);
      this._btnAdd.TabIndex = 7;
      this._btnAdd.Text = "Добавить";
      this._btnAdd.UseVisualStyleBackColor = true;
      this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
      // 
      // _gvItems
      // 
      this._gvItems.AllowUserToAddRows = false;
      this._gvItems.AllowUserToDeleteRows = false;
      this._gvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._gvItems.AutoGenerateColumns = false;
      this._gvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this._gvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn,
            this.carNameDataGridViewTextBoxColumn});
      this._gvItems.DataSource = this.objectBinder;
      this._gvItems.Location = new System.Drawing.Point(12, 27);
      this._gvItems.Name = "_gvItems";
      this._gvItems.ReadOnly = true;
      this._gvItems.Size = new System.Drawing.Size(609, 311);
      this._gvItems.TabIndex = 6;
      this._gvItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gvItems_CellDoubleClick);
      // 
      // objectBinder
      // 
      this.objectBinder.AllowEdit = false;
      this.objectBinder.AllowNew = false;
      this.objectBinder.AllowRemove = false;
      this.objectBinder.ItemType = typeof(ItemsUsage.MainForm.GridItem);
      // 
      // _mainMenu
      // 
      this._mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tooslToolStripMenuItem});
      this._mainMenu.Location = new System.Drawing.Point(0, 0);
      this._mainMenu.Name = "_mainMenu";
      this._mainMenu.Size = new System.Drawing.Size(633, 24);
      this._mainMenu.TabIndex = 10;
      this._mainMenu.Text = "_mainMenu";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
      this.fileToolStripMenuItem.Text = "&Файл";
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.exitToolStripMenuItem.Text = "Выход";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // tooslToolStripMenuItem
      // 
      this.tooslToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.carsToolStripMenuItem,
            this.inventoriesToolStripMenuItem});
      this.tooslToolStripMenuItem.Name = "tooslToolStripMenuItem";
      this.tooslToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
      this.tooslToolStripMenuItem.Text = "&Настройки";
      // 
      // carsToolStripMenuItem
      // 
      this.carsToolStripMenuItem.Name = "carsToolStripMenuItem";
      this.carsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.carsToolStripMenuItem.Text = "Машины";
      this.carsToolStripMenuItem.Click += new System.EventHandler(this.carsToolStripMenuItem_Click);
      // 
      // inventoriesToolStripMenuItem
      // 
      this.inventoriesToolStripMenuItem.Name = "inventoriesToolStripMenuItem";
      this.inventoriesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
      this.inventoriesToolStripMenuItem.Text = "Предметы";
      this.inventoriesToolStripMenuItem.Click += new System.EventHandler(this.inventoriesToolStripMenuItem_Click);
      // 
      // _btnExit
      // 
      this._btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnExit.Location = new System.Drawing.Point(546, 344);
      this._btnExit.Name = "_btnExit";
      this._btnExit.Size = new System.Drawing.Size(75, 23);
      this._btnExit.TabIndex = 11;
      this._btnExit.Text = "Выход";
      this._btnExit.UseVisualStyleBackColor = true;
      this._btnExit.Click += new System.EventHandler(this._btnExit_Click);
      // 
      // idDataGridViewTextBoxColumn
      // 
      this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
      this.idDataGridViewTextBoxColumn.HeaderText = "Номер заказа";
      this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
      this.idDataGridViewTextBoxColumn.ReadOnly = true;
      this.idDataGridViewTextBoxColumn.Width = 150;
      // 
      // dateDataGridViewTextBoxColumn
      // 
      this.dateDataGridViewTextBoxColumn.DataPropertyName = "Date";
      this.dateDataGridViewTextBoxColumn.HeaderText = "Дата";
      this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
      this.dateDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // carNameDataGridViewTextBoxColumn
      // 
      this.carNameDataGridViewTextBoxColumn.DataPropertyName = "CarName";
      this.carNameDataGridViewTextBoxColumn.HeaderText = "Машина";
      this.carNameDataGridViewTextBoxColumn.Name = "carNameDataGridViewTextBoxColumn";
      this.carNameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnExit;
      this.ClientSize = new System.Drawing.Size(633, 375);
      this.Controls.Add(this._btnExit);
      this.Controls.Add(this._btnDelete);
      this.Controls.Add(this._btnEdit);
      this.Controls.Add(this._btnAdd);
      this.Controls.Add(this._gvItems);
      this.Controls.Add(this._mainMenu);
      this.MainMenuStrip = this._mainMenu;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Заказы";
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).EndInit();
      this._mainMenu.ResumeLayout(false);
      this._mainMenu.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button _btnDelete;
    private System.Windows.Forms.Button _btnEdit;
    private System.Windows.Forms.Button _btnAdd;
    private System.Windows.Forms.DataGridView _gvItems;
    private System.Windows.Forms.MenuStrip _mainMenu;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tooslToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem carsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem inventoriesToolStripMenuItem;
    private System.Windows.Forms.Button _btnExit;
    private BLToolkit.ComponentModel.ObjectBinder objectBinder;
    private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn carNameDataGridViewTextBoxColumn;
  }
}

