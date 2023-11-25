﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductCatalog.Data.Config;

public class ItemTypeDbConfiguration : IEntityTypeConfiguration<ItemType>
{
    public static readonly string CodeSequenceName = "item_type_codes";

    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Type)
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
