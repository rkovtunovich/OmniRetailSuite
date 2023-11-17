using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class ProductItemConfiguration : IEntityTypeConfiguration<ProductItem>
{
    public void Configure(EntityTypeBuilder<ProductItem> builder)
    {
        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(100);
    }
}
