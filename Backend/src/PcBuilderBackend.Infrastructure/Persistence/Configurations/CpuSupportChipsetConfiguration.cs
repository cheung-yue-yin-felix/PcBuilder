using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuSupportChipsetConfiguration : IEntityTypeConfiguration<CpuSupportChipset>
{
    public void Configure(EntityTypeBuilder<CpuSupportChipset> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("CpuSupportChipsets");

        builder.Property(x => x.RequiresBiosUpdate)
            .IsRequired();

        builder.HasOne(x => x.Cpu)
            .WithMany(cpu => cpu.SupportedChipsets)
            .HasForeignKey(x => x.CpuId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Chipset)
            .WithMany(chipset => chipset.SupportedCpus)
            .HasForeignKey(x => x.ChipsetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CpuId, x.ChipsetId })
            .IsUnique();

        builder.HasIndex(x => x.ChipsetId);
    }
}
