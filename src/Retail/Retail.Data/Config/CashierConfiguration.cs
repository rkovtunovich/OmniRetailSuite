using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class CashierConfiguration : IEntityTypeConfiguration<Cashier>
{
    public void Configure(EntityTypeBuilder<Cashier> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(100);
    }
}
