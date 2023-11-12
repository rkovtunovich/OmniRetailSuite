using ProductCatalog.Data.Config;

namespace ProductCatalog.Data;

public class CatalogContext(DbContextOptions<CatalogContext> options) : DbContext(options)
{
    public DbSet<Item> Items { get; set; } = null!;

    public DbSet<ItemParent> ItemParents { get; set; } = null!;

    public DbSet<Brand> Brands { get; set; } = null!;

    public DbSet<ItemType> ItemTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ItemDbConfiguration());
        builder.ApplyConfiguration(new ItemParentDbConfiguration());
        builder.ApplyConfiguration(new BrandDbConfiguration());
        builder.ApplyConfiguration(new ItemTypeDbConfiguration());
    }
}
