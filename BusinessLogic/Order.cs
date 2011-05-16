using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;

using ItemsUsage.BusinessLogic.DataAccess;

namespace ItemsUsage.BusinessLogic
{
  [TableName("orders")]
  public abstract class Order : EditableObject<Order>
  {
    [PrimaryKey, NonUpdatable]
    public abstract int     Id          { get; set; }
    [MapField("car_id")]     public abstract int      CarId         { get; set; }
    [MapField("order_date")] public abstract DateTime OrderDateTime { get; set; }
  }

  public class OrderAccessor
  {
    SqlQuery<Order> _query;
    protected SqlQuery<Order> Query
    {
      get
      {
        if (_query == null)
          _query = new SqlQuery<Order>();

        return _query;
      }
    }

    public EditableList<Order> GetAll(DbManager db)
    {
      EditableList<Order> list = new EditableList<Order>();
      return Query.SelectAll(db, list);
    }

    public Order Get(DbManager db, int id)
    {
      return Query.SelectByKey(db, id);
    }

    public bool Update(DbManager db, Order item)
    {
      return Query.Update(db, item) > 0;
    }

    public bool Insert(DbManager db, Order item)
    {
      if (Query.Insert(db, item) == 0)
        return false;

      item.Id = db.SetCommand("SELECT Cast(@@IDENTITY as int)").ExecuteScalar<int>();
      return item.Id > 0;
    }

    public bool InUse(DbManager db, Order item)
    {
      return false;
    }

    public bool Delete(DbManager db, Order item)
    {
      if (InUse(db, item))
        throw new ApplicationException("This Order is in use and can't be deleted.");

      new OrderInventoryAccessor().DeleteByOrder(db, item.Id);

      return Query.Delete(db, item) > 0;
    }
  }
}
