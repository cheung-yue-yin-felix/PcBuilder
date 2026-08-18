using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardUsbConfiguration : IEntityTypeConfiguration<MotherboardUsb>
{
    public void Configure(EntityTypeBuilder<MotherboardUsb> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(usb => usb.PortCount)
            .IsRequired();

        builder.HasOne<Motherboard>()
            .WithMany(m => m.UsbPorts)
            .HasForeignKey(usb => usb.MotherboardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(usb => new { usb.MotherboardId, usb.UsbType, usb.UsbVersion })
            .IsUnique();
    }
}
