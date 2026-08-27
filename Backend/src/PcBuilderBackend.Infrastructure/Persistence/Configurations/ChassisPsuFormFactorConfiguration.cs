using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisPsuFormFactorConfiguration : IEntityTypeConfiguration<ChassisPsuFormFactor>
{
    public void Configure(EntityTypeBuilder<ChassisPsuFormFactor> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasOne(pf => pf.Chassis)
            .WithMany(c => c.PsuFormFactors)
            .HasForeignKey(pf => pf.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pf => new { pf.ChassisId, pf.PsuFormFactor })
            .IsUnique();
    }
}
