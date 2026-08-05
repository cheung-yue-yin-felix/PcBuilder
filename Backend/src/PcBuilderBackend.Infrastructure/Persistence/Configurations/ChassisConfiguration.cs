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

        builder.Property(chassis => chassis.LengthMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.WidthMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.HeightMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.MaxCpuCoolerHeightMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.MaxGraphicsCardLengthMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.MaxPsuLengthMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.MotherboardMaxWidthMm).HasPrecision(6, 2);
        builder.Property(chassis => chassis.MotherboardMaxHeightMm).HasPrecision(6, 2);

        builder.HasOne(chassis => chassis.Manufacturer)
            .WithMany(manufacturer => manufacturer.Chassis)
            .HasForeignKey(chassis => chassis.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(chassis => chassis.FanMounts)
            .WithOne(fanMount => fanMount.Chassis)
            .HasForeignKey(fm => fm.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.DriveBays)
            .WithOne(db => db.Chassis)
            .HasForeignKey(db => db.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.PcieSlots)
            .WithOne(ps => ps.Chassis)
            .HasForeignKey(ps => ps.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(chassis => chassis.Radiators)
            .WithOne(r => r.Chassis)
            .HasForeignKey(r => r.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(chassis => chassis.PsuFormFactors)
            .WithOne(ps => ps.Chassis)
            .HasForeignKey(ps => ps.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(chassis => chassis.MbFormFactors)
            .WithOne(mf => mf.Chassis)
            .HasForeignKey(mb => mb.ChassisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}