using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuConfiguration : IEntityTypeConfiguration<Cpu>
{
    public void Configure(EntityTypeBuilder<Cpu> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("Cpus");

        builder.Property(cpu => cpu.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cpu => cpu.MaxMemoryGb)
            .IsRequired();

        builder.Property(cpu => cpu.ThermalDesignPower)
            .IsRequired();

        builder.Property(cpu => cpu.PowerConsumptionWatts)
            .IsRequired();

        builder.Property(cpu => cpu.IntegratedGraphics)
            .IsRequired();

        builder.Property(cpu => cpu.IncludedStockCooler)
            .IsRequired();

        builder.HasOne(cpu => cpu.Manufacturer)
            .WithMany(manufacturer => manufacturer.Cpus)
            .HasForeignKey(cpu => cpu.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cpu => cpu.Socket)
            .WithMany(socket => socket.Cpus)
            .HasForeignKey(cpu => cpu.SocketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cpu => cpu.Series)
            .WithMany(series => series.Cpus)
            .HasForeignKey(cpu => cpu.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(cpu => cpu.RamCompats)
            .WithOne(ramCompat => ramCompat.Cpu)
            .HasForeignKey(ramCompat => ramCompat.CpuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(cpu => cpu.SupportedChipsets)
            .WithOne(support => support.Cpu)
            .HasForeignKey(support => support.CpuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cpu => cpu.ManufacturerId);
        builder.HasIndex(cpu => cpu.SocketId);
        builder.HasIndex(cpu => cpu.SeriesId);
    }
}
