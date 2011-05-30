namespace ItemsUsage.Forms
{
  partial class InventoryForm
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
      this.label2 = new System.Windows.Forms.Label();
      this._description = new System.Windows.Forms.TextBox();
      this._btnOk = new System.Windows.Forms.Button();
      this._btnCancel = new System.Windows.Forms.Button();
      this._code = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this._price = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 41);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(63, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description:";
      // 
      // _description
      // 
      this._description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._description.Location = new System.Drawing.Point(89, 38);
      this._description.Name = "_description";
      this._description.Size = new System.Drawing.Size(438, 20);
      this._description.TabIndex = 1;
      // 
      // _btnOk
      // 
      this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnOk.Location = new System.Drawing.Point(369, 86);
      this._btnOk.Name = "_btnOk";
      this._btnOk.Size = new System.Drawing.Size(75, 23);
      this._btnOk.TabIndex = 3;
      this._btnOk.Text = "&OK";
      this._btnOk.UseVisualStyleBackColor = true;
      this._btnOk.Click += new System.EventHandler(this._btnOk_Click);
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(450, 86);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 4;
      this._btnCancel.Text = "&Cancel";
      this._btnCancel.UseVisualStyleBackColor = true;
      // 
      // _code
      // 
      this._code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this._code.Location = new System.Drawing.Point(89, 12);
      this._code.MaxLength = 4000;
      this._code.Name = "_code";
      this._code.Size = new System.Drawing.Size(235, 20);
      this._code.TabIndex = 0;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(16, 15);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(35, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Code:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(16, 65);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(34, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Price:";
      // 
      // _price
      // 
      this._price.Location = new System.Drawing.Point(89, 62);
      this._price.Name = "_price";
      this._price.Size = new System.Drawing.Size(100, 20);
      this._price.TabIndex = 2;
      // 
      // InventoryForm
      // 
      this.AcceptButton = this._btnOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(536, 112);
      this.Controls.Add(this._price);
      this.Controls.Add(this.label4);
      this.Controls.Add(this._code);
      this.Controls.Add(this.label3);
      this.Controls.Add(this._btnOk);
      this.Controls.Add(this._btnCancel);
      this.Controls.Add(this._description);
      this.Controls.Add(this.label2);
      this.Name = "InventoryForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Inventory";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox _description;
    private System.Windows.Forms.Button _btnOk;
    private System.Windows.Forms.Button _btnCancel;
    private System.Windows.Forms.TextBox _code;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox _price;
  }
}