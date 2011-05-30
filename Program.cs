using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

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
      try
      {
        DbManager.AddDataProvider(new SqlCeDataProvider());
        string dbPath = GetApplicationDirectory() + "\\ItemsUsage.sdf";
        DbManager.AddConnectionString("SqlCe", "", @"Data Source=" + dbPath);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public static string GetApplicationDirectory()
    {
      return Path.GetDirectoryName(Application.ExecutablePath);
    }
  }
}
