using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class PcBuildUserConfiguration : IEntityTypeConfiguration<PcBuildUser>
{
    public void Configure(EntityTypeBuilder<PcBuildUser> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("PcBuildUsers");

        builder.HasIndex(user => user.PcBuildId).IsUnique();
        builder.HasIndex(user => user.UserId);
    }
}
