using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Users;

namespace RepairIT.UserService;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Id);

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
    }
}
