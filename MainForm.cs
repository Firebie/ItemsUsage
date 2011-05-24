using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

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

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void carsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new CarsForm(_model).ShowDialog();
    }

    private void inventoriesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new InventoriesForm(_model).ShowDialog();
    }

    private void _btnExit_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      Order item = TypeAccessor<Order>.CreateInstanceEx();
      item.OrderDateTime = DateTime.Today;

      using (OrderForm form = new OrderForm(_model, item, new EditableList<OrderInventory>()))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          item.AcceptChanges();
          //objectBinder.List.Add(new GridItem(item, GetInventoryName(item.InventoryId)));
        }
      }
    }
  }
}
