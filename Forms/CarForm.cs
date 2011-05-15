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
  public partial class CarForm : Form
  {
    Car _item;
    bool _newItem;
    public CarForm(Car item, bool newItem)
    {
      _item    = item;
      _newItem = newItem;
      InitializeComponent();

      _description.Focus();

      if (!_newItem)
        _id.Text = _item.Id.ToString();

      _description.Text = _item.Description;
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      _item.Description = _description.Text.Trim();
      bool ok = false;
      using (DbManager db = new DbManager())
        if (_newItem)
          ok = new CarAccessor().Insert(db, _item);
        else
          ok = new CarAccessor().Update(db, _item);

      if (ok)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}
