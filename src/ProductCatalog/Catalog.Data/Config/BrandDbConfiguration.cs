﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Core.Entities.ProductAggregate;

namespace ProductCatalog.Data.Config;

public class BrandDbConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
           .IsRequired();

        builder.Property(cb => cb.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
