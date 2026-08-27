using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisDriveBayConfiguration : IEntityTypeConfiguration<ChassisDriveBay>
{
    public void Configure(EntityTypeBuilder<ChassisDriveBay> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(d => d.BayCount)
            .IsRequired();

        builder.HasOne(d => d.Chassis)
            .WithMany(c => c.DriveBays)
            .HasForeignKey(d => d.ChassisId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => new { d.ChassisId, d.DriveBayFormFactor })
            .IsUnique();
    }
}