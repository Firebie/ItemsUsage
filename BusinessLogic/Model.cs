using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Data;
using BLToolkit.EditableObjects;
using BLToolkit.Reflection;

namespace ItemsUsage.BusinessLogic
{
  public class Model
  {
    public Car CarCreateInstance()
    {
      return TypeAccessor<Car>.CreateInstanceEx();
    }

    public Car GetCar(int id)
    {
      using (DbManager db = new DbManager())
        return new CarAccessor().Get(db, id);
    }

    public EditableList<Car> CarGetAll()
    {
      using (DbManager db = new DbManager())
        return new CarAccessor().GetAll(db);
    }

    public bool CarUpdate(Car item)
    {
      using (DbManager db = new DbManager())
        return new CarAccessor().Update(db, item);
    }

    public bool CarInsert(Car item)
    {
      using (DbManager db = new DbManager())
        return new CarAccessor().Insert(db, item);
    }

    public bool CarDelete(Car item)
    {
      using (DbManager db = new DbManager())
        return new CarAccessor().Delete(db, item);
    }

    public Inventory InventoryCreateInstance()
    {
      return TypeAccessor<Inventory>.CreateInstanceEx();
    }

    public EditableList<Inventory> InventoryGetAll()
    {
      using (DbManager db = new DbManager())
        return new InventoryAccessor().GetAll(db);
    }

    public Inventory GetInventory(int id)
    {
      using (DbManager db = new DbManager())
        return new InventoryAccessor().Get(db, id);
    }

    public bool InventoryDelete(Inventory item)
    {
      using (DbManager db = new DbManager())
        return new InventoryAccessor().Delete(db, item);
    }

    public EditableList<OrderInventory> OrderInventoryGetAll()
    {
      using (DbManager db = new DbManager())
        return new OrderInventoryAccessor().GetAll(db);
    }

    public EditableList<OrderInventory> OrderInventoryGetByOrder(int orderId)
    {
      using (DbManager db = new DbManager())
        return new OrderInventoryAccessor().GetByOrderId(db, orderId);
    }

    public bool OrderInventoryInsert(OrderInventory item)
    {
      using (DbManager db = new DbManager())
        return new OrderInventoryAccessor().Insert(db, item);
    }

    public bool OrderInventoryUpdate(OrderInventory item)
    {
      using (DbManager db = new DbManager())
        return new OrderInventoryAccessor().Update(db, item);
    }
  }
}
