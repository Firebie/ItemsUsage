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
  [TableName("order_inventories")]
  public abstract class OrderInventory : EditableObject<OrderInventory>
  {
    [PrimaryKey, NonUpdatable]
    public abstract int Id { get; set; }

    [MapField("order_id")]           public abstract int      OrderId           { get; set; }
    [MapField("sequence_id")]        public abstract int      SequenceId        { get; set; }
    [MapField("inventory_id")]       public abstract int      InventoryId       { get; set; }
    [MapField("inventory_quantity")] public abstract int      InventoryQuantity { get; set; }
    [MapField("inventory_price")]    public abstract decimal  InventoryPrice    { get; set; }
    [MapField("inventory_date")]     public abstract DateTime InventoryDate     { get; set; }
  }

  public class OrderInventoryAccessor
  {
    SqlQuery<OrderInventory> _query;
    protected SqlQuery<OrderInventory> Query
    {
      get
      {
        if (_query == null)
          _query = new SqlQuery<OrderInventory>();

        return _query;
      }
    }

    public EditableList<OrderInventory> GetAll(DbManager db)
    {
      EditableList<OrderInventory> list = new EditableList<OrderInventory>();
      return Query.SelectAll(db, list);
    }

    public EditableList<OrderInventory> GetByOrderId(DbManager db, int orderId)
    {
      string sql = 
        "select * from order_inventories where order_id = " 
          + db.DataProvider.Convert("OrderId", ConvertType.NameToQueryParameter)
        + " order by sequence_id";

      EditableList<OrderInventory> list = new EditableList<OrderInventory>();
      db.SetCommand(sql, db.Parameter("OrderId", orderId)).ExecuteList<OrderInventory>(list);
      return list;
    }

    public OrderInventory Get(DbManager db, int id)
    {
      return Query.SelectByKey(db, id);
    }

    public bool Update(DbManager db, OrderInventory item)
    {
      return Query.Update(db, item) > 0;
    }

    public bool Insert(DbManager db, OrderInventory item)
    {
      if (Query.Insert(db, item) == 0)
        return false;

      item.Id = db.SetCommand("SELECT Cast(@@IDENTITY as int)").ExecuteScalar<int>();
      return item.Id > 0;
    }

    public bool Delete(DbManager db, OrderInventory item)
    {
      return Query.Delete(db, item) > 0;
    }

    public bool DeleteByOrder(DbManager db, int orderId)
    {
      string sql = "delete from order_items where order_id = "
        + db.DataProvider.Convert("OrderId", ConvertType.NameToQueryParameter);

      return db.SetCommand(sql, db.Parameter("OrderId", orderId)).ExecuteNonQuery() > 0;
    }
  }
}
