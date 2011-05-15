using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Data;
using BLToolkit.Reflection;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class InventoriesForm : Form
  {
    public InventoriesForm()
    {
      InitializeComponent();
      using (DbManager db = new DbManager())
        objectBinder.List = new InventoryAccessor().GetAll(db);
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      Inventory item = TypeAccessor<Inventory>.CreateInstanceEx();

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
          using (DbManager db = new DbManager())
            new InventoryAccessor().Delete(db, item);
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
  }
}
