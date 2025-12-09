using Microsoft.EntityFrameworkCore;

namespace RedisExample.API.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Kalem", Price = 10 },
            new Product { Id = 2, Name = "Macbook", Price = 10000 },
            new Product { Id = 3, Name = "Playstation", Price = 5000 }
        );
        base.OnModelCreating(modelBuilder);
    }
}
