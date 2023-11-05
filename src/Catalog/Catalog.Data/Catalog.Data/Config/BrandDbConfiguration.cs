using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Config;

public class BrandDbConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .UseHiLo("brand_hilo")
           .IsRequired();

        builder.Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
