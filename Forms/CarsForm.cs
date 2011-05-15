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
    public CarsForm()
    {
      InitializeComponent();
      using (DbManager db = new DbManager())
        objectBinder.List = new CarAccessor().GetAll(db);
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      Car item = TypeAccessor<Car>.CreateInstanceEx();

      using (CarForm form = new CarForm(item, true))
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
      using (CarForm form = new CarForm(item, false))
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
  }
}
