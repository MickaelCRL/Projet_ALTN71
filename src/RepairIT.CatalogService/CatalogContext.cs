using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Computers;

namespace RepairIT.CatalogService;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    public virtual DbSet<Computer> Computers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Computer>().HasKey(c => c.Id);

        modelBuilder.Entity<Computer>().Property(c => c.Type).HasConversion<string>();
    }
}
