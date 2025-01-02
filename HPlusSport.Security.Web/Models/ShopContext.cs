using Microsoft.EntityFrameworkCore;

namespace HPlusSport.Security.Web.Models;

public class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options) :
        base(options)
    { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(a => a.Category)
            .HasForeignKey(a => a.CategoryId);

        modelBuilder.Entity<Order>()
            .HasMany(c => c.Products)
            .WithMany(p => p.Orders);

        modelBuilder.Entity<Order>()
            .HasOne(c => c.User);

        modelBuilder.RemovePluralizingTableNameConvention();

        modelBuilder.Seed();
    }
}
