using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItemsUsage.BusinessLogic;
using ItemsUsage.Forms;

namespace ItemsUsage
{
  public partial class MainForm : Form
  {
    Model _model = new Model();
    public MainForm()
    {
      InitializeComponent();
    }

    private void _btnCars_Click(object sender, EventArgs e)
    {
      new CarsForm(_model).ShowDialog();
    }

    private void _btnInventories_Click(object sender, EventArgs e)
    {
      new InventoriesForm(_model).ShowDialog();
    }
  }
}
