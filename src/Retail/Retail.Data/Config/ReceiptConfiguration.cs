using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public static readonly string CodeSequenceName = "receipt_codes";

    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Receipt.ReceiptItems));

        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Cashier)
            .WithMany()
            .HasForeignKey(x => x.CashierId)
            .IsRequired();

        builder.HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(x => x.StoreId)
            .IsRequired();

        builder.Property(x => x.TotalPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Date)
            .IsRequired(true);

        builder.Property(e => e.CodeNumber)
            .HasDefaultValueSql($"nextval('\"{CodeSequenceName}\"')");

        builder.Property(p => p.CodePrefix)
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.CodePrefix, e.CodeNumber })
            .IsUnique();

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
