namespace ItemsUsage.Forms
{
  partial class OrderReportForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderReportForm));
      this._btnCancel = new System.Windows.Forms.Button();
      this._report = new FastReport.Report();
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this._report)).BeginInit();
      this.SuspendLayout();
      // 
      // _btnCancel
      // 
      this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this._btnCancel.Location = new System.Drawing.Point(747, 387);
      this._btnCancel.Name = "_btnCancel";
      this._btnCancel.Size = new System.Drawing.Size(75, 23);
      this._btnCancel.TabIndex = 2;
      this._btnCancel.Text = "&Close";
      this._btnCancel.UseVisualStyleBackColor = true;
      this._btnCancel.Click += new System.EventHandler(this._btnCancel_Click);
      // 
      // _report
      // 
      this._report.ReportResourceString = resources.GetString("_report.ReportResourceString");
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(24, 22);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(75, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "button1";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // OrderReportForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this._btnCancel;
      this.ClientSize = new System.Drawing.Size(834, 422);
      this.Controls.Add(this.button1);
      this.Controls.Add(this._btnCancel);
      this.Name = "OrderReportForm";
      this.Text = "Order report";
      ((System.ComponentModel.ISupportInitialize)(this._report)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button _btnCancel;
    private FastReport.Report _report;
    private System.Windows.Forms.Button button1;

  }
}