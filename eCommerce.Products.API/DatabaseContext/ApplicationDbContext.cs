using eCommerce.Products.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Products.API.DatabaseContext;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<Product> Products { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);
    //     
    // }
}