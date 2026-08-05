using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisRadiatorConfiguration : IEntityTypeConfiguration<ChassisRadiator>
{
    public void Configure(EntityTypeBuilder<ChassisRadiator> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(r => r.RadiatorCount)
            .IsRequired();

        builder.HasOne(r => r.Chassis)
            .WithMany(c => c.Radiators)
            .HasForeignKey(r => r.ChassisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.ChassisId, r.Length, r.MountLocation })
            .IsUnique();
    }
}