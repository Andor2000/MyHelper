using Microsoft.EntityFrameworkCore;
using MyHelper.Data;
using MyHelper.Enums;
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace MyHelper
{
    public static class Program
    {
        /// <summary>
        /// Наименование файла БД.
        /// </summary>
        public static readonly string _dataBaseName = "MyDatabase.sqlite";

        /// <summary>
        /// Путь к базе данных.
        /// </summary>
        private static readonly string databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _dataBaseName);

        private static readonly ProgramContext _context = new ProgramContext();

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Create_E_sqlite3();
            CreateDataBase();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormScriptMerge(_context));
        }

        private static void Create_E_sqlite3()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "e_sqlite3.dll");

            if (!File.Exists(path))
            {
                File.WriteAllBytes(path, Files.e_sqlite3);
            }
        }

        /// <summary>
        /// Создание базы данных и применение миграций.
        /// </summary>
        private static void CreateDataBase()
        {
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
            }

            _context.Database.Migrate();
        }
    }
}
