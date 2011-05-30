namespace ItemsUsage.Forms
{
  partial class OrderForm
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
      this._gvItems = new System.Windows.Forms.DataGridView();
      this.sequenceIdDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.itemInventoryDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.itemInventoryPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.itemInventoryQuantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.TotalPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.objectBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
      this._btnCancel = new System.Windows.Forms.Button();
      this._btnAdd = new System.Windows.Forms.Button();
      this._btnEdit = new System.Windows.Forms.Button();
      this._btnDelete = new System.Windows.Forms.Button();
      this._btnOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this._selectCar = new System.Windows.Forms.Button();
      this._car = new System.Windows.Forms.TextBox();
      this._allTotal = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this._date = new System.Windows.Forms.DateTimePicker();
      this._btnPrint = new System.Windows.Forms.Button();
      this._btnDesign = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).BeginInit();
      this.SuspendLayout();
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
            this.sequenceIdDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.itemInventoryDateDataGridViewTextBoxColumn,
            this.itemInventoryPriceDataGridViewTextBoxColumn,
            this.itemInventoryQuantityDataGridViewTextBoxColumn,
            this.TotalPrice});
      this._gvItems.DataSource = this.objectBinder;
      this._gvItems.Location = new System.Drawing.Point(12, 42);
      this._gvItems.MultiSelect = false;
      this._gvItems.Name = "_gvItems";
      this._gvItems.ReadOnly = true;
      this._gvItems.Size = new System.Drawing.Size(643, 243);
      this._gvItems.TabIndex = 0;
      this._gvItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gvItems_CellDoubleClick);
      // 
      // sequenceIdDataGridViewTextBoxColumn
      // 
      this.sequenceIdDataGridViewTextBoxColumn.DataPropertyName = "SequenceId";
      this.sequenceIdDataGridViewTextBoxColumn.HeaderText = "Sequence Id";
      this.sequenceIdDataGridViewTextBoxColumn.Name = "sequenceIdDataGridViewTextBoxColumn";
      this.sequenceIdDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // nameDataGridViewTextBoxColumn
      // 
      this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
      this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
      this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
      this.nameDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // itemInventoryDateDataGridViewTextBoxColumn
      // 
      this.itemInventoryDateDataGridViewTextBoxColumn.DataPropertyName = "Date";
      this.itemInventoryDateDataGridViewTextBoxColumn.HeaderText = "Date";
      this.itemInventoryDateDataGridViewTextBoxColumn.Name = "itemInventoryDateDataGridViewTextBoxColumn";
      this.itemInventoryDateDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // itemInventoryPriceDataGridViewTextBoxColumn
      // 
      this.itemInventoryPriceDataGridViewTextBoxColumn.DataPropertyName = "Price";
      this.itemInventoryPriceDataGridViewTextBoxColumn.HeaderText = "Price";
      this.itemInventoryPriceDataGridViewTextBoxColumn.Name = "itemInventoryPriceDataGridViewTextBoxColumn";
      this.itemInventoryPriceDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // itemInventoryQuantityDataGridViewTextBoxColumn
      // 
      this.itemInventoryQuantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
      this.itemInventoryQuantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
      this.itemInventoryQuantityDataGridViewTextBoxColumn.Name = "itemInventoryQuantityDataGridViewTextBoxColumn";
      this.itemInventoryQuantityDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // TotalPrice
      // 
      this.TotalPrice.DataPropertyName = "TotalPrice";
      this.TotalPrice.HeaderText = "TotalPrice";
      this.TotalPrice.Name = "TotalPrice";
      this.TotalPrice.ReadOnly = true;
      // 
      // objectBinder
      // 
      this.objectBinder.AllowEdit = false;
      this.objectBinder.AllowNew = false;
      this.objectBinder.AllowRemove = false;
      this.objectBinder.ItemType = typeof(ItemsUsage.Forms.OrderForm.GridItem);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(580, 291);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _btnAdd
      // 
      this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnAdd.Location = new System.Drawing.Point(13, 291);
      this._btnAdd.Name = "_btnAdd";
      this._btnAdd.Size = new System.Drawing.Size(75, 23);
      this._btnAdd.TabIndex = 3;
      this._btnAdd.Text = "Add";
      this._btnAdd.UseVisualStyleBackColor = true;
      this._btnAdd.Click += new System.EventHandler(this._btnAdd_Click);
      // 
      // _btnEdit
      // 
      this._btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnEdit.Location = new System.Drawing.Point(94, 291);
      this._btnEdit.Name = "_btnEdit";
      this._btnEdit.Size = new System.Drawing.Size(75, 23);
      this._btnEdit.TabIndex = 4;
      this._btnEdit.Text = "Edit";
      this._btnEdit.UseVisualStyleBackColor = true;
      this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
      // 
      // _btnDelete
      // 
      this._btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnDelete.Location = new System.Drawing.Point(175, 291);
      this._btnDelete.Name = "_btnDelete";
      this._btnDelete.Size = new System.Drawing.Size(75, 23);
      this._btnDelete.TabIndex = 5;
      this._btnDelete.Text = "Delete";
      this._btnDelete.UseVisualStyleBackColor = true;
      this._btnDelete.Click += new System.EventHandler(this._btnDelete_Click);
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOk.Location = new System.Drawing.Point(499, 291);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 6;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(26, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Car:";
      // 
      // _selectCar
      // 
      this._selectCar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._selectCar.Location = new System.Drawing.Point(305, 8);
      this._selectCar.Name = "_selectCar";
      this._selectCar.Size = new System.Drawing.Size(23, 23);
      this._selectCar.TabIndex = 8;
      this._selectCar.Text = "...";
      this._selectCar.UseVisualStyleBackColor = true;
      this._selectCar.Click += new System.EventHandler(this._selectCar_Click);
      // 
      // _car
      // 
      this._car.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._car.Location = new System.Drawing.Point(45, 10);
      this._car.Name = "_car";
      this._car.ReadOnly = true;
      this._car.Size = new System.Drawing.Size(254, 20);
      this._car.TabIndex = 9;
      // 
      // _allTotal
      // 
      this._allTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._allTotal.Location = new System.Drawing.Point(550, 10);
      this._allTotal.Name = "_allTotal";
      this._allTotal.ReadOnly = true;
      this._allTotal.Size = new System.Drawing.Size(105, 20);
      this._allTotal.TabIndex = 11;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(496, 13);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(44, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "All total:";
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(334, 13);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(33, 13);
      this.label3.TabIndex = 12;
      this.label3.Text = "Date:";
      // 
      // _date
      // 
      this._date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this._date.Location = new System.Drawing.Point(382, 10);
      this._date.Name = "_date";
      this._date.Size = new System.Drawing.Size(100, 20);
      this._date.TabIndex = 13;
      // 
      // _btnPrint
      // 
      this._btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnPrint.Location = new System.Drawing.Point(373, 291);
      this._btnPrint.Name = "_btnPrint";
      this._btnPrint.Size = new System.Drawing.Size(75, 23);
      this._btnPrint.TabIndex = 14;
      this._btnPrint.Text = "&Print";
      this._btnPrint.UseVisualStyleBackColor = true;
      this._btnPrint.Click += new System.EventHandler(this._btnPrint_Click);
      // 
      // _btnDesign
      // 
      this._btnDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnDesign.Location = new System.Drawing.Point(292, 291);
      this._btnDesign.Name = "_btnDesign";
      this._btnDesign.Size = new System.Drawing.Size(75, 23);
      this._btnDesign.TabIndex = 15;
      this._btnDesign.Text = "&Design";
      this._btnDesign.UseVisualStyleBackColor = true;
      this._btnDesign.Click += new System.EventHandler(this._btnDesign_Click);
      // 
      // OrderForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(667, 318);
      this.Controls.Add(this._btnDesign);
      this.Controls.Add(this._btnPrint);
      this.Controls.Add(this._date);
      this.Controls.Add(this.label3);
      this.Controls.Add(this._allTotal);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._selectCar);
      this.Controls.Add(this._car);
      this.Controls.Add(this.label1);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._btnDelete);
      this.Controls.Add(this._btnEdit);
      this.Controls.Add(this._btnAdd);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._gvItems);
      this.Name = "OrderForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Order inventories";
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView _gvItems;
    private System.Windows.Forms.Button _btnCancel;
    private BLToolkit.ComponentModel.ObjectBinder objectBinder;
    private System.Windows.Forms.Button _btnAdd;
    private System.Windows.Forms.Button _btnEdit;
    private System.Windows.Forms.Button _btnDelete;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.DataGridViewTextBoxColumn sequenceIdDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn itemInventoryDateDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn itemInventoryPriceDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn itemInventoryQuantityDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn TotalPrice;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _selectCar;
    private System.Windows.Forms.TextBox _car;
    private System.Windows.Forms.TextBox _allTotal;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.DateTimePicker _date;
    private System.Windows.Forms.Button _btnPrint;
    private System.Windows.Forms.Button _btnDesign;
  }
}