using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Retail.Core.Entities;

namespace Retail.Data.Config;

public class StoreConfiguration: IEntityTypeConfiguration<Store>
{
    public static readonly string CodeSequenceName = "store_codes";

    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cb => cb.Address)
            .HasMaxLength(100);

        builder.HasMany(x => x.Cashiers)
            .WithMany();

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.Property(e => e.CodeNumber)
            .HasDefaultValueSql($"nextval('\"{CodeSequenceName}\"')");

        builder.Property(p => p.CodePrefix)
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.CodePrefix, e.CodeNumber })
            .IsUnique();
    }
}
