using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class ReceiptItemConfiguration : IEntityTypeConfiguration<ReceiptItem>
{
    public void Configure(EntityTypeBuilder<ReceiptItem> builder)
    {
        builder.Property(x => x.Id)
           .IsRequired();

        builder.Property(x => x.ReceiptId)
            .IsRequired();

        builder.HasOne(x => x.CatalogItem)
            .WithMany()
            .HasForeignKey(x => x.CatalogItemId)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired(true)
            .HasColumnType("numeric(18,3)");

        builder.Property(x => x.UnitPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.LineNumber)
            .IsRequired(true);
    }
}
