using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace ItemsUsage.BusinessLogic.DataAccess
{
  public class SqlQueryExt<T> : SqlQuery<T>
  {
    #region InsertAndGetIdentity

    public virtual int InsertAndGetIdentity(DbManager db, object obj)
    {
      SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "InsertAndGetIdentity");

      return db
        .SetCommand(query.QueryText, query.GetParameters(db, obj))
        .ExecuteScalar<int>();
    }

    public virtual int InsertAndGetIdentity(object obj)
    {
      DbManager db = GetDbManager();

      try
      {
        return InsertAndGetIdentity(db, obj);
      }
      finally
      {
        Dispose(db);
      }
    }

    protected override SqlQueryInfo CreateSqlText(DbManager db, Type type, string actionName)
    {
      switch (actionName)
      {
        case "InsertAndGetIdentity":
          SqlQueryInfo qi = CreateInsertSqlText(db, type, -1);

          qi.QueryText += "\nSELECT Cast(@@IDENTITY as int)";

          return qi;

        default:
          return base.CreateSqlText(db, type, actionName);
      }
    }

    #endregion
  }
}
