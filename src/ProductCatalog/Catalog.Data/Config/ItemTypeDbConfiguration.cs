using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Data.Config;

public class ItemTypeDbConfiguration : IEntityTypeConfiguration<ItemType>
{
    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}
