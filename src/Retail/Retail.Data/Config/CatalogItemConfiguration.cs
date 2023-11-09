using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Retail.Data.Config;

public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(100);
    }
}
