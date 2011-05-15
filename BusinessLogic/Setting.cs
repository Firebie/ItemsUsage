using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace ItemsUsage.BusinessLogic
{
  [TableName("settings")]
  public abstract class Setting
  {
    [PrimaryKey]
    public abstract string Name { get; set; }
    public abstract string Value { get; set; }
  }

  public class SettingAccessor
  {
    SqlQuery<Setting> _query;
    protected SqlQuery<Setting> Query
    {
      get
      {
        if (_query == null)
          _query = new SqlQuery<Setting>();

        return _query;
      }
    }

    public Setting Get(DbManager db, string name)
    {
      return Query.SelectByKey(db, name);
    }

    public bool Update(DbManager db, Setting item)
    {
      return Query.Update(db, item) > 0;
    }

    public bool Insert(DbManager db, Setting item)
    {
      return Query.Insert(db, item) > 0;
    }
  }
}
