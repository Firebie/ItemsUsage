﻿namespace ItemsUsage.Forms
{
  partial class SelectCarForm
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
      this._btnCancel = new System.Windows.Forms.Button();
      this._btnOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this._filter = new System.Windows.Forms.TextBox();
      this.objectBinder = new BLToolkit.ComponentModel.ObjectBinder(this.components);
      this.displayStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.displayStringDataGridViewTextBoxColumn});
      this._gvItems.DataSource = this.objectBinder;
      this._gvItems.Location = new System.Drawing.Point(12, 42);
      this._gvItems.Name = "_gvItems";
      this._gvItems.ReadOnly = true;
      this._gvItems.Size = new System.Drawing.Size(443, 244);
      this._gvItems.TabIndex = 0;
      this._gvItems.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this._gvItems_CellDoubleClick);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(380, 291);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 1;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnOk.Location = new System.Drawing.Point(299, 292);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 2;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(50, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Фильтр:";
      // 
      // _filter
      // 
      this._filter.Location = new System.Drawing.Point(68, 10);
      this._filter.Name = "_filter";
      this._filter.Size = new System.Drawing.Size(387, 20);
      this._filter.TabIndex = 4;
      this._filter.TextChanged += new System.EventHandler(this._filter_TextChanged);
      // 
      // objectBinder
      // 
      this.objectBinder.AllowEdit = false;
      this.objectBinder.AllowNew = false;
      this.objectBinder.AllowRemove = false;
      this.objectBinder.ItemType = typeof(ItemsUsage.BusinessLogic.Car);
      // 
      // displayStringDataGridViewTextBoxColumn
      // 
      this.displayStringDataGridViewTextBoxColumn.DataPropertyName = "DisplayString";
      this.displayStringDataGridViewTextBoxColumn.HeaderText = "Название";
      this.displayStringDataGridViewTextBoxColumn.Name = "displayStringDataGridViewTextBoxColumn";
      this.displayStringDataGridViewTextBoxColumn.ReadOnly = true;
      this.displayStringDataGridViewTextBoxColumn.Width = 300;
      // 
      // SelectCarForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(467, 318);
      this.Controls.Add(this._filter);
      this.Controls.Add(this.label1);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._gvItems);
      this.Name = "SelectCarForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Выбор машины";
      ((System.ComponentModel.ISupportInitialize)(this._gvItems)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.DataGridView _gvItems;
    private System.Windows.Forms.Button _btnCancel;
    private BLToolkit.ComponentModel.ObjectBinder objectBinder;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _filter;
    private System.Windows.Forms.DataGridViewTextBoxColumn displayStringDataGridViewTextBoxColumn;
  }
}