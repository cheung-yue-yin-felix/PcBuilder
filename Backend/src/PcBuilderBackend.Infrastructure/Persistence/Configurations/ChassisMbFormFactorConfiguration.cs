using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisMbFormFactorConfiguration : IEntityTypeConfiguration<ChassisMbFormFactor>
{
    public void Configure(EntityTypeBuilder<ChassisMbFormFactor> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("ChassisMbFormFactor");

        builder.HasOne(mf => mf.Chassis)
            .WithMany(c => c.MbFormFactors)
            .HasForeignKey(mf => mf.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mf => new { mf.ChassisId, mf.MbFormFactor })
            .IsUnique();
    }
}
