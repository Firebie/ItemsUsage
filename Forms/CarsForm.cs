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
  public partial class CarsForm : Form
  {
    Model _model;
    public CarsForm(Model model)
    {
      _model = model;
      InitializeComponent();
      objectBinder.List = _model.CarGetAll();
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      Car item = _model.CarCreateInstance();

      using (CarForm form = new CarForm(_model, item, true))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          item.AcceptChanges();
          objectBinder.List.Add(item);
        }
      }
    }

    private void Edit(Car item)
    {
      using (CarForm form = new CarForm(_model, item, false))
      {
        if (form.ShowDialog() != DialogResult.OK)
          item.RejectChanges();
      }
    }

    private void _btnEdit_Click(object sender, EventArgs e)
    {
      if (_gvItems.CurrentRow != null)
        Edit((Car)_gvItems.CurrentRow.DataBoundItem);
    }

    private void _gvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      DataGridViewRow row = _gvItems.Rows[e.RowIndex];

      Edit((Car)row.DataBoundItem);
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

        Car item = (Car)_gvItems.CurrentRow.DataBoundItem;

        try
        {
          UseWaitCursor = true;
          _model.CarDelete(item);
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
