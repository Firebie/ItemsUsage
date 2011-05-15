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
      this._btnCars = new System.Windows.Forms.Button();
      this._btnInventories = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // _btnCars
      // 
      this._btnCars.Location = new System.Drawing.Point(12, 12);
      this._btnCars.Name = "_btnCars";
      this._btnCars.Size = new System.Drawing.Size(75, 23);
      this._btnCars.TabIndex = 0;
      this._btnCars.Text = "Cars";
      this._btnCars.UseVisualStyleBackColor = true;
      this._btnCars.Click += new System.EventHandler(this._btnCars_Click);
      // 
      // _btnInventories
      // 
      this._btnInventories.Location = new System.Drawing.Point(93, 12);
      this._btnInventories.Name = "_btnInventories";
      this._btnInventories.Size = new System.Drawing.Size(75, 23);
      this._btnInventories.TabIndex = 1;
      this._btnInventories.Text = "Inventories";
      this._btnInventories.UseVisualStyleBackColor = true;
      this._btnInventories.Click += new System.EventHandler(this._btnInventories_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(292, 273);
      this.Controls.Add(this._btnInventories);
      this.Controls.Add(this._btnCars);
      this.Name = "MainForm";
      this.Text = "Main";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnCars;
    private System.Windows.Forms.Button _btnInventories;
  }
}

