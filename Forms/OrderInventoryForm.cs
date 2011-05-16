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
  public partial class OrderInventoryForm : Form
  {
    Model          _model;
    OrderInventory _item;
    bool           _newItem;
    public OrderInventoryForm(Model model, OrderInventory item, bool newItem)
    {
      _model   = model;
      _item    = item;
      _newItem = newItem;

      InitializeComponent();

      _orderId.Text = _item.OrderId.ToString();
      _sequenceId.Text = _item.SequenceId.ToString();
      Inventory inventory = _model.GetInventory(_item.InventoryId);
      _inventory.Text = inventory != null ? inventory.DisplayString : string.Empty;
      _quantity.Text = _item.InventoryQuantity.ToString();
      _price.Text    = _item.InventoryPrice.ToString();
      _date.Value    = _item.InventoryDate;
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      if (_item.InventoryId == 0)
      {
        MessageBox.Show("The inventory is not specified", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      int quantity = 0;
      if (!int.TryParse(_quantity.Text, out quantity) || quantity < 0)
      {
        MessageBox.Show("The item quantity has an error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      _item.InventoryQuantity = quantity;

      decimal price = 0;
      if (!decimal.TryParse(_price.Text, out price) || price < 0)
      {
        MessageBox.Show("The price has an error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      _item.InventoryPrice = price;

      bool ok = false;
      if (_newItem)
        ok = _model.OrderInventoryInsert(_item);
      else
        ok = _model.OrderInventoryUpdate(_item);

      if (ok)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void _selectInventory_Click(object sender, EventArgs e)
    {
      using (SelectInventoryForm form = new SelectInventoryForm(_model))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          _item.InventoryId    = form.SelectedItem.Id;
          _item.InventoryPrice = form.SelectedItem.Price;

          _inventory.Text = form.SelectedItem.DisplayString;
          _price.Text = _item.InventoryPrice.ToString();
        }
      }
    }
  }
}
