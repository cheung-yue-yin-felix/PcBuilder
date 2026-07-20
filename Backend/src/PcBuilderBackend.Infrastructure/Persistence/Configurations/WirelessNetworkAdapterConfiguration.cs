using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class WirelessNetworkAdapterConfiguration : IEntityTypeConfiguration<WirelessNetworkAdapter>
{
    public void Configure(EntityTypeBuilder<WirelessNetworkAdapter> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.MaxSpeedMbps)
            .IsRequired();

        builder.HasOne(w => w.Manufacturer)
            .WithMany(m => m.WirelessNetworkAdapters)
            .HasForeignKey(w => w.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}