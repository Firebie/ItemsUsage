using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.EditableObjects;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class SelectInventoryForm : Form
  {
    Model _model;
    EditableList<Inventory> _list;

    Inventory _selectedItem;

    public Inventory SelectedItem
    {
      get
      {
        return _selectedItem;
      }
    }

    public SelectInventoryForm(Model model)
    {
      _model = model;
      InitializeComponent();
      _list = _model.InventoryGetAll();
      Filter();
    }

    void Filter()
    {
      EditableList<Inventory> list;
      string filter = _filter.Text.Trim();
      if (string.IsNullOrEmpty(filter))
        list = _list;
      else
      {
        list = new EditableList<Inventory>();
        foreach (Inventory item in _list)
        {
          if (item.DisplayString.IndexOf(filter) != -1)
            list.Add(item);
        }
      }
      objectBinder.List = list;
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      Inventory item = _model.InventoryCreateInstance();

      using (InventoryForm form = new InventoryForm(item, true))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          item.AcceptChanges();
          objectBinder.List.Add(item);
        }
      }
    }

    private void Edit(Inventory item)
    {
      using (InventoryForm form = new InventoryForm(item, false))
      {
        if (form.ShowDialog() != DialogResult.OK)
          item.RejectChanges();
      }
    }

    private void _btnEdit_Click(object sender, EventArgs e)
    {
      if (_gvItems.CurrentRow != null)
        Edit((Inventory)_gvItems.CurrentRow.DataBoundItem);
    }

    private void _gvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      DataGridViewRow row = _gvItems.Rows[e.RowIndex];

      Edit((Inventory)row.DataBoundItem);
    }

    private void _btnDelete_Click(object sender, EventArgs e)
    {
      if (_gvItems.CurrentRow != null)
      {
        DialogResult dr =
          MessageBox.Show(
            "Are you sure to delete the item?",
            "Warning",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question);

        if (dr == DialogResult.Cancel)
          return;

        Inventory item = (Inventory)_gvItems.CurrentRow.DataBoundItem;

        try
        {
          UseWaitCursor = true;
          _model.InventoryDelete(item);
          UseWaitCursor = false;

          objectBinder.List.Remove(item);
        }
        catch (Exception ex)
        {
          UseWaitCursor = false;
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      if (_gvItems.CurrentRow != null)
      {
        _selectedItem = (Inventory)_gvItems.CurrentRow.DataBoundItem;

        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void _filter_TextChanged(object sender, EventArgs e)
    {
      Filter();
    }
  }
}
