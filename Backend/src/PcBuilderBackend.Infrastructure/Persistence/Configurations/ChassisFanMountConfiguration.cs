using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisFanMountConfiguration : IEntityTypeConfiguration<ChassisFanMount>
{
    public void Configure(EntityTypeBuilder<ChassisFanMount> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasOne<Chassis>()
            .WithMany(c => c.FanMounts)
            .HasForeignKey(f => f.ChassisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}