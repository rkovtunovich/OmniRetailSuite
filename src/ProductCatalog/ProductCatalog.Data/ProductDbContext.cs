using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductCatalog.Data.Config;

namespace ProductCatalog.Data;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
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

        // Define a conversion for all DateTimeOffset properties
        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, DateTime>(
            v => v.UtcDateTime,  // Convert to UTC DateTime when saving to the database
            v => new DateTimeOffset(v, TimeSpan.Zero));  // Convert back to DateTimeOffset when reading from the database

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset) || property.ClrType == typeof(DateTimeOffset?))
                    property.SetValueConverter(dateTimeOffsetConverter);
            }
        }

        builder.HasSequence<int>(BrandDbConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);

        builder.HasSequence<int>(ItemDbConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);

        builder.HasSequence<int>(ItemParentDbConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);

        builder.HasSequence<int>(ItemTypeDbConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<EntityBase>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    entry.Property(nameof(EntityBase.CreatedAt)).IsModified = false;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
