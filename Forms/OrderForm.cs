using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using BLToolkit.Data;
using BLToolkit.EditableObjects;
using BLToolkit.Reflection;

using FastReport;

using ItemsUsage.BusinessLogic;

namespace ItemsUsage.Forms
{
  public partial class OrderForm : Form
  {
    public class GridItem
    {
      string _name;
      OrderInventory _item;
      public GridItem(OrderInventory item, string name)
      {
        _name = name;
        _item = item;
      }

      public OrderInventory Item
      {
        get
        {
          return _item;
        }
      }

      public int SequenceId
      {
        get
        {
          return _item.SequenceId;
        }
      }

      public string Name
      {
        get
        {
          return _name;
        }
      }

      public decimal Price
      {
        get
        {
          return _item.InventoryPrice;
        }
      }

      public int Quantity
      {
        get
        {
          return _item.InventoryQuantity;
        }
      }

      public decimal TotalPrice
      {
        get
        {
          return Quantity * Price;
        }
      }

      public DateTime Date
      {
        get
        {
          return _item.InventoryDate;
        }
      }
    }

    Model _model;
    Order _order;
    bool _new = false;
    Dictionary<int, Inventory> _inventories = new Dictionary<int, Inventory>();
    EditableList<GridItem> _gridItems = new EditableList<GridItem>();
    public OrderForm(Model model, Order order, EditableList<OrderInventory> items)
    {
      _model = model;
      _order = order;
      
      _new = _order.Id == 0;
      
      foreach (Inventory item in _model.InventoryGetAll())
        _inventories.Add(item.Id, item);
      
      foreach (OrderInventory item in items)
        _gridItems.Add(new GridItem(item, GetInventoryName(item.InventoryId)));
      _gridItems.AcceptChanges();

      InitializeComponent();

      _date.Value = _order.OrderDateTime;
      objectBinder.List = _gridItems;
      SetCar();
      SetAllTotal();
    }

    string GetInventoryName(int id)
    {
      string name = "(no name)";
      Inventory inv;
      if (_inventories.TryGetValue(id, out inv))
        name = inv.DisplayString;

      return name;
    }

    private void _btnAdd_Click(object sender, EventArgs e)
    {
      OrderInventory item = TypeAccessor<OrderInventory>.CreateInstanceEx();
      item.SequenceId = objectBinder.List.Count + 1;
      item.InventoryQuantity = 1;
      item.InventoryDate = _date.Value.Date;
      if (objectBinder.List.Count > 0)
        item.InventoryDate = ((GridItem)objectBinder.List[objectBinder.List.Count - 1]).Item.InventoryDate;

      using (OrderInventoryForm form = new OrderInventoryForm(_model, item, true))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          item.AcceptChanges();
          objectBinder.List.Add(new GridItem(item, GetInventoryName(item.InventoryId)));
          SetAllTotal();
        }
      }
    }

    private void Edit(OrderInventory item)
    {
      using (OrderInventoryForm form = new OrderInventoryForm(_model, item, false))
      {
        if (form.ShowDialog() != DialogResult.OK)
          item.RejectChanges();
        else
          SetAllTotal();
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

        objectBinder.List.Remove((GridItem)_gvItems.CurrentRow.DataBoundItem);
        int seq = 0;
        foreach (GridItem item in objectBinder.List)
        {
          seq += 1;
          item.Item.SequenceId = seq;
        }
        SetAllTotal();
      }
    }

    bool Save()
    {
      _order.OrderDateTime = _date.Value.Date;
      try
      {
        UseWaitCursor = true;

        using (DbManager db = new DbManager())
        {
          db.BeginTransaction();

          if (_new)
            new OrderAccessor().Insert(db, _order);
          else
            new OrderAccessor().Update(db, _order);

          OrderInventoryAccessor acc = new OrderInventoryAccessor();
          foreach (GridItem item in _gridItems)
          {
            item.Item.OrderId = _order.Id;
            if (_gridItems.NewItems.Contains(item))
              acc.Insert(db, item.Item);
            else
              acc.Update(db, item.Item);
          }

          foreach (GridItem item in _gridItems.DelItems)
            acc.Delete(db, item.Item);

          db.CommitTransaction();
        }

        UseWaitCursor = false;

      }
      catch (Exception ex)
      {
        UseWaitCursor = false;
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    private void _btnOk_Click(object sender, EventArgs e)
    {
      if (Save())
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    void SetCar()
    {
      Car car = _model.GetCar(_order.CarId);
      _car.Text = car != null ? car.DisplayString : "(no car selected)";
    }

    private void _selectCar_Click(object sender, EventArgs e)
    {
      using (SelectCarForm form = new SelectCarForm(_model))
      {
        if (form.ShowDialog() == DialogResult.OK)
        {
          _order.CarId = form.SelectedItem.Id;
          SetCar();
        }
      }
    }

    decimal GetTotal()
    {
      decimal total = 0;
      foreach (GridItem item in objectBinder.List)
        total += item.TotalPrice;

      return total;
    }

    void SetAllTotal()
    {
      _allTotal.Text = GetTotal().ToString();
    }

    void PrepareReport(Report report)
    {
      Car car = _model.GetCar(_order.CarId);
      report.Load(Program.GetApplicationDirectory() + "\\Order.frx");
      report.SetParameterValue("OrderDate", _order.OrderDateTime);
      report.SetParameterValue("OrderCar", car != null ? car.DisplayString : string.Empty);
      report.SetParameterValue("OrderPrice", GetTotal());
      report.RegisterData(_gridItems, "data");
    }

    private void _btnPrint_Click(object sender, EventArgs e)
    {
      if (Save())
      {
        try
        {
          using (Report report = new Report())
          {
            PrepareReport(report);
            report.Show();
          }
        }
        catch (System.Exception ex)
        {
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void _btnDesign_Click(object sender, EventArgs e)
    {
      if (Save())
      {
        try
        {
          using (Report report = new Report())
          {
            PrepareReport(report);
            report.Design(true);
          }
        }
        catch (System.Exception ex)
        {
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }
  }
}
