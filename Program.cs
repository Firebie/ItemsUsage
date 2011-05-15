using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace ItemsUsage
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      DbManager.AddDataProvider(new SqlCeDataProvider());
      DbManager.AddConnectionString("SqlCe", "", @"Data Source=C:\MyUsers\programs\manysrc\projects\ItemsUsage\db\ItemsUsage.sdf");
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}
