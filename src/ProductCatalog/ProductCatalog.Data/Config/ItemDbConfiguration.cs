using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductCatalog.Data.Config;

public class ItemDbConfiguration : IEntityTypeConfiguration<ProductItem>
{
    public static readonly string CodeSequenceName = "item_codes";

    public void Configure(EntityTypeBuilder<ProductItem> builder)
    {
        builder.ToTable("items");

        builder.Property(ci => ci.Id)
            .IsRequired();

        builder.Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(ci => ci.Description)
            .IsRequired(false)
            .HasMaxLength(4000);

        builder.Property(ci => ci.Price)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");

        builder.Property(ci => ci.PictureUri)
            .IsRequired(false);

        builder.HasOne(ci => ci.ProductBrand)
            .WithMany()
            .HasForeignKey(ci => ci.ProductBrandId);

        builder.HasOne(ci => ci.ProductType)
            .WithMany()
            .HasForeignKey(ci => ci.ProductTypeId);

        builder.HasOne(ci => ci.Parent)
            .WithMany()
            .HasForeignKey(ci => ci.ParentId);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.Property(e => e.CodeNumber)
            .HasDefaultValueSql($"nextval('\"{CodeSequenceName}\"')");

        builder.Property(p => p.CodePrefix)
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.CodePrefix, e.CodeNumber })
            .IsUnique();
    }
}
