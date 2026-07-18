using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisConfiguration : IEntityTypeConfiguration<Chassis>
{
    public void Configure(EntityTypeBuilder<Chassis> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(chassis => chassis.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Map enum lists to Postgres enum arrays
        builder.Property(chassis => chassis.SupportedMbFormFactors)
            .HasColumnType("mb_form_factor[]");

        builder.Property(chassis => chassis.SupportedPsuFormFactors)
            .HasColumnType("psu_form_factor[]");

        builder.HasOne(chassis => chassis.Manufacturer)
            .WithMany(manufacturer => manufacturer.Chassis)
            .HasForeignKey(chassis => chassis.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(chassis => chassis.FanMounts)
            .WithOne()
            .HasForeignKey(fm => fm.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.DriveBays)
            .WithOne()
            .HasForeignKey(db => db.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.PcieSlots)
            .WithOne()
            .HasForeignKey(ps => ps.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.Radiators)
            .WithOne()
            .HasForeignKey(r => r.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}