using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardConfiguration : IEntityTypeConfiguration<Motherboard>
{
    public void Configure(EntityTypeBuilder<Motherboard> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(motherboard => motherboard.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(motherboard => motherboard.WidthMm)
            .HasPrecision(6, 2);
        
        builder.Property(motherboard => motherboard.HeightMm)
            .HasPrecision(6, 2);

        builder.HasOne(motherboard => motherboard.Chipset)
            .WithMany(chipset => chipset.Motherboards)
            .HasForeignKey(motherboard => motherboard.ChipsetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(motherboard => motherboard.Socket)
            .WithMany(socket => socket.Motherboards)
            .HasForeignKey(motherboard => motherboard.SocketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(motherboard => motherboard.Manufacturer)
            .WithMany(manufacturer => manufacturer.Motherboards)
            .HasForeignKey(motherboard => motherboard.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(motherboard => motherboard.PcieSlots)
            .WithOne()
            .HasForeignKey(pcieSlot => pcieSlot.MotherboardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(motherboard => motherboard.M2Slots)
            .WithOne()
            .HasForeignKey(m2Slot => m2Slot.MotherboardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}