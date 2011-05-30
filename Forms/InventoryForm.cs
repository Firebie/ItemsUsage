using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Data;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class InventoryForm : Form
  {
    Inventory _item;
    bool _newItem;
    public InventoryForm(Inventory item, bool newItem)
    {
      _item    = item;
      _newItem = newItem;
      InitializeComponent();

      _code.Text = _item.Code;
      _description.Text = _item.Description;
      _price.Text = _item.Price.ToString();
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      _item.Code = _code.Text.Trim();
      _item.Description = _description.Text.Trim();
      decimal price = 0;
      if (!decimal.TryParse(_price.Text, out price) || price < 0)
      {
        MessageBox.Show("The price has an error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      _item.Price = price;
      bool ok = false;
      using (DbManager db = new DbManager())
        if (_newItem)
          ok = new InventoryAccessor().Insert(db, _item);
        else
          ok = new InventoryAccessor().Update(db, _item);

      if (ok)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}
