using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisFanMountConfiguration : IEntityTypeConfiguration<ChassisFanMount>
{
    public void Configure(EntityTypeBuilder<ChassisFanMount> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasOne(f => f.Chassis)
            .WithMany(c => c.FanMounts)
            .HasForeignKey(f => f.ChassisId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(f => f.Options)
            .WithOne(o => o.Mount)
            .HasForeignKey(o => o.ChassisFanMountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(f => new { f.ChassisId, f.Location })
            .IsUnique();
    }
}