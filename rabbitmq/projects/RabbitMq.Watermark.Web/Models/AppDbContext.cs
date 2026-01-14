using System;
using Microsoft.EntityFrameworkCore;

namespace RabbitMq.Watermark.Web.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {

    }
    public DbSet<Product> Products { get; set; }
}
