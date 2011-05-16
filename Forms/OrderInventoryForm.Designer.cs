﻿namespace ItemsUsage.Forms
{
  partial class OrderInventoryForm
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
      this.label1 = new System.Windows.Forms.Label();
      this._orderId = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this._inventory = new System.Windows.Forms.TextBox();
      this._btnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this._price = new System.Windows.Forms.TextBox();
      this._sequenceId = new System.Windows.Forms.TextBox();
      this._selectInventory = new System.Windows.Forms.Button();
      this._quantity = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this._date = new System.Windows.Forms.DateTimePicker();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Order Id:";
      // 
      // _orderId
      // 
      this._orderId.Location = new System.Drawing.Point(86, 9);
      this._orderId.Name = "_orderId";
      this._orderId.ReadOnly = true;
      this._orderId.Size = new System.Drawing.Size(100, 20);
      this._orderId.TabIndex = 5;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 64);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(54, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Inventory:";
      // 
      // _inventory
      // 
      this._inventory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._inventory.Location = new System.Drawing.Point(86, 61);
      this._inventory.Name = "_inventory";
      this._inventory.ReadOnly = true;
      this._inventory.Size = new System.Drawing.Size(418, 20);
      this._inventory.TabIndex = 1;
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOk.Location = new System.Drawing.Point(369, 168);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 4;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(450, 168);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 5;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(13, 38);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(59, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Sequence:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(13, 116);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(34, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Price:";
      // 
      // _price
      // 
      this._price.Location = new System.Drawing.Point(86, 113);
      this._price.Name = "_price";
      this._price.Size = new System.Drawing.Size(100, 20);
      this._price.TabIndex = 2;
      // 
      // _sequenceId
      // 
      this._sequenceId.Location = new System.Drawing.Point(86, 35);
      this._sequenceId.Name = "_sequenceId";
      this._sequenceId.ReadOnly = true;
      this._sequenceId.Size = new System.Drawing.Size(100, 20);
      this._sequenceId.TabIndex = 7;
      // 
      // _selectInventory
      // 
      this._selectInventory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this._selectInventory.Location = new System.Drawing.Point(510, 59);
      this._selectInventory.Name = "_selectInventory";
      this._selectInventory.Size = new System.Drawing.Size(23, 23);
      this._selectInventory.TabIndex = 0;
      this._selectInventory.Text = "...";
      this._selectInventory.UseVisualStyleBackColor = true;
      this._selectInventory.Click += new System.EventHandler(this._selectInventory_Click);
      // 
      // _quantity
      // 
      this._quantity.Location = new System.Drawing.Point(86, 87);
      this._quantity.Name = "_quantity";
      this._quantity.Size = new System.Drawing.Size(100, 20);
      this._quantity.TabIndex = 1;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(13, 90);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(49, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Quantity:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(12, 144);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(33, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "Date:";
      // 
      // _date
      // 
      this._date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this._date.Location = new System.Drawing.Point(86, 138);
      this._date.Name = "_date";
      this._date.Size = new System.Drawing.Size(100, 20);
      this._date.TabIndex = 3;
      // 
      // OrderInventoryForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(536, 194);
      this.Controls.Add(this._date);
      this.Controls.Add(this.label6);
      this.Controls.Add(this._quantity);
      this.Controls.Add(this.label5);
      this.Controls.Add(this._selectInventory);
      this.Controls.Add(this._sequenceId);
      this.Controls.Add(this._price);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._inventory);
      this.Controls.Add(this.label2);
      this.Controls.Add(this._orderId);
      this.Controls.Add(this.label1);
      this.Name = "OrderInventoryForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Order Inventory";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox _orderId;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _inventory;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _price;
    private System.Windows.Forms.TextBox _sequenceId;
    private System.Windows.Forms.Button _selectInventory;
    private System.Windows.Forms.TextBox _quantity;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.DateTimePicker _date;
  }
}