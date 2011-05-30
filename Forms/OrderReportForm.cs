using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastReport;

using BLToolkit.Data;
using BLToolkit.EditableObjects;
using BLToolkit.Reflection;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class OrderReportForm : Form
  {
    Order _order;
    EditableList<OrderInventory> _items;
    public OrderReportForm(Order order, EditableList<OrderInventory> items)
    {
      _order = order;
      _items = items;
      InitializeComponent();
    }

    private void _btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      using (Report report = new Report())
      {
        report.Load(@"C:\MyUsers\programs\manysrc\projects\ItemsUsage\Reports\Order.frx");
        report.SetParameterValue("OrderDate", _order.OrderDateTime);
        report.SetParameterValue("OrderCar", _order.CarId);
        report.Design(true);
        //report.Show();
      }
    }
  }
}
