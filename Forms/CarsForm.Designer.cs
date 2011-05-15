namespace ItemsUsage.Forms
{
  partial class CarsForm
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
      this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.objectBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
      this._btnCancel = new System.Windows.Forms.Button();
      this._btnOk = new System.Windows.Forms.Button();
      this._btnAdd = new System.Windows.Forms.Button();
      this._btnEdit = new System.Windows.Forms.Button();
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
            this.idDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
      this._gvItems.DataSource = this.objectBinder;
      this._gvItems.Location = new System.Drawing.Point(12, 2);
      this._gvItems.Name = "_gvItems";
      this._gvItems.ReadOnly = true;
      this._gvItems.Size = new System.Drawing.Size(405, 198);
      this._gvItems.TabIndex = 0;
      this._gvItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gvItems_CellDoubleClick);
      // 
      // idDataGridViewTextBoxColumn
      // 
      this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
      this.idDataGridViewTextBoxColumn.HeaderText = "Id";
      this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
      this.idDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // descriptionDataGridViewTextBoxColumn
      // 
      this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
      this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
      this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
      this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
      // 
      // objectBinder
      // 
      this.objectBinder.AllowEdit = false;
      this.objectBinder.AllowNew = false;
      this.objectBinder.AllowRemove = false;
      this.objectBinder.ItemType = typeof(ItemsUsage.BusinessLogic.Car);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.Location = new System.Drawing.Point(342, 206);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOk.Location = new System.Drawing.Point(261, 206);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      // 
      // _btnAdd
      // 
      this._btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this._btnAdd.Location = new System.Drawing.Point(13, 205);
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
      this._btnEdit.Location = new System.Drawing.Point(94, 205);
      this._btnEdit.Name = "_btnEdit";
      this._btnEdit.Size = new System.Drawing.Size(75, 23);
      this._btnEdit.TabIndex = 4;
      this._btnEdit.Text = "Edit";
      this._btnEdit.UseVisualStyleBackColor = true;
      this._btnEdit.Click += new System.EventHandler(this._btnEdit_Click);
      // 
      // CarsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(429, 232);
      this.Controls.Add(this._btnEdit);
      this.Controls.Add(this._btnAdd);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._gvItems);
      this.Name = "CarsForm";
      this.Text = "Cars";
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView _gvItems;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.Button _btnOk;
    private BLToolkit.ComponentModel.ObjectBinder objectBinder;
    private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
    private System.Windows.Forms.Button _btnAdd;
    private System.Windows.Forms.Button _btnEdit;
  }
}