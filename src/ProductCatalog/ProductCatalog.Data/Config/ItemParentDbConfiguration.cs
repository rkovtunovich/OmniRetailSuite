using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductCatalog.Data.Config;

public class ItemParentDbConfiguration : IEntityTypeConfiguration<ItemParent>
{
    public static readonly string CodeSequenceName = "item_parent_codes";

    public void Configure(EntityTypeBuilder<ItemParent> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasOne(ci => ci.Parent)
            .WithMany()
            .HasForeignKey(ci => ci.ParentId);

        builder.HasOne(ci => ci.Parent)
               .WithMany(p => p.Children)
               .HasForeignKey(ci => ci.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.Property(cb => cb.CodeNumber)
            .HasDefaultValueSql($"nextval('\"{CodeSequenceName}\"')");

        builder.Property(p => p.CodePrefix)
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.CodePrefix, e.CodeNumber })
            .IsUnique();
    }
}
