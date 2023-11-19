using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Receipt.ReceiptItems));

        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Cashier)
            .WithMany()
            .HasForeignKey(x => x.CashierId)
            .IsRequired();

        builder.Property(x => x.TotalPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Number)
            .IsRequired()
            .HasMaxLength(9);

        builder.Property(x => x.Date)
            .IsRequired(true);

        builder.Property(x => x.Number)
            .IsRequired(true)
            .HasMaxLength(9);

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
