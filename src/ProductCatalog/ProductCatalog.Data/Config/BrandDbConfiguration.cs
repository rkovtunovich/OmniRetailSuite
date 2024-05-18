using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductCatalog.Data.Config;

public class BrandDbConfiguration : IEntityTypeConfiguration<Brand>
{
    public static readonly string CodeSequenceName = "brand_codes";

    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.Property(e => e.CodeNumber)
            .HasDefaultValueSql($"nextval('\"{CodeSequenceName}\"')");

        builder.Property(p => p.CodePrefix)
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.CodePrefix, e.CodeNumber })
            .IsUnique();
    }
}
