using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Retail.Core.Entities;

namespace Retail.Data.Config;

public class AppClientSettingsConfiguration : IEntityTypeConfiguration<AppClientSettings>
{
    public void Configure(EntityTypeBuilder<AppClientSettings> builder)
    {
        builder.HasOne(x => x.Store)
            .WithMany()
            .HasForeignKey(x => x.StoreId)
            .IsRequired();

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
