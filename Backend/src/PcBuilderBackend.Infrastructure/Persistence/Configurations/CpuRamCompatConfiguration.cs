using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuRamCompatConfiguration : IEntityTypeConfiguration<CpuRamCompat>
{
    public void Configure(EntityTypeBuilder<CpuRamCompat> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("CpuRamCompats");

        builder.Property(r => r.DdrGeneration)
            .IsRequired();

        builder.Property(r => r.RamModuleCount)
            .IsRequired();

        builder.Property(r => r.RamRank)
            .IsRequired();

        builder.Property(r => r.MaxSpeedMts)
            .IsRequired();

        builder.HasOne(r => r.Cpu)
            .WithMany(c => c.RamCompats)
            .HasForeignKey(r => r.CpuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(r => new { r.CpuId, r.DdrGeneration, r.RamModuleCount, r.RamRank })
            .IsUnique();
    }
}
