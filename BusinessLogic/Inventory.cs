using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;

using ItemsUsage.BusinessLogic.DataAccess;

namespace ItemsUsage.BusinessLogic
{
  [TableName("inventories")]
  public abstract class Inventory : EditableObject<Inventory>
  {
    [PrimaryKey, NonUpdatable]
    public abstract int     Id          { get; set; }
    public abstract string  Code        { get; set; }
    public abstract string  Description { get; set; }
    public abstract decimal Price       { get; set; }
  }

  public class InventoryAccessor
  {
    SqlQuery<Inventory> _query;
    protected SqlQuery<Inventory> Query
    {
      get
      {
        if (_query == null)
          _query = new SqlQuery<Inventory>();

        return _query;
      }
    }

    public EditableList<Inventory> GetAll(DbManager db)
    {
      EditableList<Inventory> list = new EditableList<Inventory>();
      return Query.SelectAll(db, list);
    }

    public Inventory Get(DbManager db, int id)
    {
      return Query.SelectByKey(db, id);
    }

    public bool Update(DbManager db, Inventory item)
    {
      return Query.Update(db, item) > 0;
    }

    public bool Insert(DbManager db, Inventory item)
    {
      if (Query.Insert(db, item) == 0)
        return false;

      item.Id = db.SetCommand("SELECT Cast(@@IDENTITY as int)").ExecuteScalar<int>();
      return item.Id > 0;
    }

    public bool InUse(DbManager db, Inventory item)
    {
      string sql = "select count(*) from order_items where inventory_id = "
        + db.DataProvider.Convert("InvId", ConvertType.NameToQueryParameter);

      return db.SetCommand(sql, db.Parameter("InvId", item.Id)).ExecuteScalar<int>() > 0;
    }

    public bool Delete(DbManager db, Inventory item)
    {
      if (InUse(db, item))
        throw new ApplicationException("This inventory is in use and can't be deleted.");
      
      return Query.Delete(db, item) > 0;
    }
  }
}
