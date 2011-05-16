using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class CarForm : Form
  {
    Model _model;
    Car   _item;
    bool  _newItem;
    public CarForm(Model model, Car item, bool newItem)
    {
      _model   = model;
      _item    = item;
      _newItem = newItem;
      InitializeComponent();

      if (_newItem)
        _id.Text = "(new item)";
      else
        _id.Text = _item.Id.ToString();

      _description.Text = _item.Description;
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      _item.Description = _description.Text.Trim();
      bool ok = false;
      if (_newItem)
        ok = _model.CarInsert(_item);
      else
        ok = _model.CarUpdate(_item);

      if (ok)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }
  }
}
