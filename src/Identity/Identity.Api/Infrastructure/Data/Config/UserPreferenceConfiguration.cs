using Identity.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Api.Infrastructure.Data.Config;

public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.Property(p => p.Settings)
            .HasColumnType("jsonb");

        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}
