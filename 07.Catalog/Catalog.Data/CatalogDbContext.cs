using Catalog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data;

public class CatalogDbContext : DbContext
{
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }
}
