using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Data.Config;

public class ItemParentDbConfiguration : IEntityTypeConfiguration<ItemParent>
{
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
    }
}
