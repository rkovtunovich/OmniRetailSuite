using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Retail.Data.Config;

namespace Retail.Data;

public class RetailDbContext(DbContextOptions<RetailDbContext> options) : DbContext(options)
{
    public DbSet<Receipt> Receipts { get; set; } = null!;

    public DbSet<ReceiptItem> ReceiptItems { get; set; } = null!;

    public DbSet<Cashier> Cashiers { get; set; } = null!;

    public DbSet<ProductItem> ProductItems { get; set; } = null!;

    public DbSet<Store> Stores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ReceiptConfiguration());
        builder.ApplyConfiguration(new ReceiptItemConfiguration());
        builder.ApplyConfiguration(new CashierConfiguration());
        builder.ApplyConfiguration(new ProductItemConfiguration());
        builder.ApplyConfiguration(new StoreConfiguration());

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

        builder.HasSequence<int>(ReceiptConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);

        builder.HasSequence<int>(CashierConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);

        builder.HasSequence<int>(StoreConfiguration.CodeSequenceName)
            .StartsAt(1)
            .IncrementsBy(1);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
