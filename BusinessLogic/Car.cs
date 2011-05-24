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
  [TableName("cars")]
  public abstract class Car : EditableObject<Car>
  {
    [PrimaryKey, NonUpdatable]
    public abstract int Id { get; set; }
    public abstract string Description { get; set; }

    [MapIgnore]
    public string DisplayString
    {
      get
      {
        return Description;
      }
    }
  }

  public class CarAccessor
  {
    SqlQuery<Car> _query;
    protected SqlQuery<Car> Query
    {
      get
      {
        if (_query == null)
          _query = new SqlQuery<Car>();

        return _query;
      }
    }

    public EditableList<Car> GetAll(DbManager db)
    {
      EditableList<Car> list = new EditableList<Car>();
      return Query.SelectAll(db, list);
    }

    public Car Get(DbManager db, int id)
    {
      return Query.SelectByKey(db, id);
    }

    public bool Update(DbManager db, Car item)
    {
      return Query.Update(db, item) > 0;
    }

    public bool Insert(DbManager db, Car item)
    {
      if (Query.Insert(db, item) == 0)
        return false;

      item.Id = db.SetCommand("SELECT Cast(@@IDENTITY as int)").ExecuteScalar<int>();
      return item.Id > 0;
    }

    public bool InUse(DbManager db, Car item)
    {
      string sql = "select count(*) from orders where car_id = " 
        + db.DataProvider.Convert("CarId", ConvertType.NameToQueryParameter);

      return db.SetCommand(sql, db.Parameter("CarId", item.Id)).ExecuteScalar<int>() > 0;
    }
    
    public bool Delete(DbManager db, Car item)
    {
      if (InUse(db, item))
        throw new ApplicationException("This car is in use and can't be deleted.");

      return Query.Delete(db, item) > 0;
    }
  }
}
