using Microsoft.EntityFrameworkCore;
using MyHelper.Models.Entity;
    
namespace MyHelper.Data
{
    public class ProgramContext : DbContext
    {
        public ProgramContext(DbContextOptions<ProgramContext> options) : base(options)
        {
        }

        public DbSet<ColomnEntity> Colomns { get; set; }
        public DbSet<TableEntity> Tables { get; set; }
    }
}
