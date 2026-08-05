using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardPcieConfiguration : IEntityTypeConfiguration<MotherboardPcie>
{
    public void Configure(EntityTypeBuilder<MotherboardPcie> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(p => p.SlotCount)
            .IsRequired();

        builder.HasOne<Motherboard>()
            .WithMany(m => m.PcieSlots)
            .HasForeignKey(p => p.MotherboardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => new { p.MotherboardId, p.SlotType, p.SlotLanes, p.Generation })
            .IsUnique();
    }
}