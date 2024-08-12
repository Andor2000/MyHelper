using Microsoft.EntityFrameworkCore;
using MyHelper.Models.Entity;
using System;
using System.IO;

namespace MyHelper.Data
{
    /// <summary>
    /// Контекст данных.
    /// </summary>
    public class ProgramContext : DbContext
    {
        /// <summary>
        /// Таблицы.
        /// </summary>
        public DbSet<TableEntity> Tables { get; set; }

        /// <summary>
        /// Колоноки.
        /// </summary>
        public DbSet<ColomnEntity> Colomns { get; set; }

        /// <summary>
        /// Настройки.
        /// </summary>
        public DbSet<SettingEntity> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Program._dataBaseName);
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка схемы базы данных
            base.OnModelCreating(modelBuilder);
        }
    }
}
