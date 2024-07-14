using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Retail.Core.Entities;

namespace Retail.Data.Config;

public class CashierConfiguration : IEntityTypeConfiguration<Cashier>
{
    public static readonly string CodeSequenceName = "cashier_codes";

    public void Configure(EntityTypeBuilder<Cashier> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired(true)
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
