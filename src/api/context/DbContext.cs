using Microsoft.EntityFrameworkCore;
using System;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> Items => Set<OrderItem>();

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Mouse", UnitPrice = 12.90m, AvailableQuantity = 15 },
            new Product { Id = 2, ProductName = "Keyboard", UnitPrice = 169.90m, AvailableQuantity = 200 },
            new Product { Id = 3, ProductName = "Microfone", UnitPrice = 34.90m, AvailableQuantity = 22 },
            new Product { Id = 4, ProductName = "HeadPhone", UnitPrice = 115.90m, AvailableQuantity = 15 },
            new Product { Id = 5, ProductName = "Sound box", UnitPrice = 12.90m, AvailableQuantity = 15 },
            new Product { Id = 6, ProductName = "Monitor", UnitPrice = 169.90m, AvailableQuantity = 200 },
            new Product { Id = 7, ProductName = "Microphone Stand", UnitPrice = 34.90m, AvailableQuantity = 22 },
            new Product { Id = 8, ProductName = "Mug", UnitPrice = 115.90m, AvailableQuantity = 15 },
            new Product { Id = 9, ProductName = "Special Coffee", UnitPrice = 12.90m, AvailableQuantity = 15 },
            new Product { Id = 10, ProductName = "Webcam", UnitPrice = 169.90m, AvailableQuantity = 200 },
            new Product { Id = 11, ProductName = "MousePad", UnitPrice = 34.90m, AvailableQuantity = 22 },
            new Product { Id = 12, ProductName = "Computer Case", UnitPrice = 115.90m, AvailableQuantity = 15 },
            new Product { Id = 13, ProductName = "GPU", UnitPrice = 12.90m, AvailableQuantity = 15 },
            new Product { Id = 14, ProductName = "CPU", UnitPrice = 169.90m, AvailableQuantity = 200 },
            new Product { Id = 15, ProductName = "RAM memory", UnitPrice = 34.90m, AvailableQuantity = 22 }

        );
    }
}