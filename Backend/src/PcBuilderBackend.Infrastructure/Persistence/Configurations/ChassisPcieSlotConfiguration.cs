using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class ChassisPcieSlotConfiguration : IEntityTypeConfiguration<ChassisPcieSlot>
{
    public void Configure(EntityTypeBuilder<ChassisPcieSlot> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(p => p.SlotCount)
            .IsRequired();

        builder.HasOne<Chassis>()
            .WithMany(c => c.PcieSlots)
            .HasForeignKey(p => p.ChassisId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}