using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItemsUsage.Forms;

namespace ItemsUsage
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void _btnCars_Click(object sender, EventArgs e)
    {
      new CarsForm().ShowDialog();
    }
  }
}
