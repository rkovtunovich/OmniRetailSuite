using Retail.Data.Config;

namespace Retail.Data;

public class RetailDbContext(DbContextOptions<RetailDbContext> options) : DbContext(options)
{
    public DbSet<Receipt> Receipts { get; set; } = null!;

    public DbSet<ReceiptItem> ReceiptItems { get; set; } = null!;

    public DbSet<Cashier> Cashiers { get; set; } = null!;

    public DbSet<CatalogItem> CatalogItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ReceiptConfiguration());
        builder.ApplyConfiguration(new ReceiptItemConfiguration());
        builder.ApplyConfiguration(new CashierConfiguration());
        builder.ApplyConfiguration(new CatalogItemConfiguration());
    }
}
