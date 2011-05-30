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
using BLToolkit.EditableObjects;

using ItemsUsage.BusinessLogic;
using ItemsUsage.Forms;
using ItemsUsage.Properties;

namespace ItemsUsage
{
  public partial class MainForm : Form
  {
    public class GridItem
    {
      string _carName;
      Order _item;
      public GridItem(Order item, string carName)
      {
        _carName = carName;
        _item    = item;
      }

      public int Id
      {
        get
        {
          return _item.Id;
        }
      }

      public string CarName
      {
        get
        {
          return _carName;
        }
      }

      public DateTime Date
      {
        get
        {
          return _item.OrderDateTime;
        }
      }

      public Order Item
      {
        get
        {
          return _item;
        }
      }
    }

    Model _model = new Model();
    Dictionary<int, Car> _cars = new Dictionary<int, Car>();
    public MainForm()
    {
      InitializeComponent();

      carsToolStripMenuItem.Text = Resources.Cars;
      inventoriesToolStripMenuItem.Text = Resources.Inventories;

      InitCars();
      ShowOrders();
    }

    void InitCars()
    {
      _cars.Clear();
      foreach (Car car in _model.CarGetAll())
        _cars.Add(car.Id, car);
    }

    void ShowOrders()
    {
      EditableList<GridItem> items = new EditableList<GridItem>();
      using (DbManager db = new DbManager())
        foreach (Order order in new OrderAccessor().GetAll(db))
          items.Add(new GridItem(order, GetCarName(order.CarId)));


      objectBinder.List = items;
    }

    string GetCarName(int id)
    {
      string name = "no car";
      Car car;
      if (_cars.TryGetValue(id, out car))
        name = car.DisplayString;

      return name;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void carsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      new CarsForm(_model).ShowDialog();
      InitCars();
      ShowOrders();
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
          objectBinder.List.Add(new GridItem(item, GetCarName(item.CarId)));
        }
      }
    }

    private void Edit(Order item)
    {
      using (OrderForm form = new OrderForm(_model, item, _model.OrderInventoryGetByOrder(item.Id)))
      {
        if (form.ShowDialog() != DialogResult.OK)
          item.RejectChanges();
        else
          ShowOrders();
      }
    }

    private void _btnEdit_Click(object sender, EventArgs e)
    {
      if (_gvItems.CurrentRow != null)
        Edit(((GridItem)_gvItems.CurrentRow.DataBoundItem).Item);
    }

    private void _gvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        DataGridViewRow row = _gvItems.Rows[e.RowIndex];

        Edit(((GridItem)row.DataBoundItem).Item);
      }
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

        GridItem gridItem = (GridItem)_gvItems.CurrentRow.DataBoundItem;

        try
        {
          UseWaitCursor = true;
          using (DbManager db = new DbManager())
          {
            db.BeginTransaction();
            new OrderAccessor().Delete(db, gridItem.Item);
            db.CommitTransaction();
          }
          UseWaitCursor = false;

          objectBinder.List.Remove(gridItem);
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
