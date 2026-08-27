using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class WiredNetworkAdapterConfiguration : IEntityTypeConfiguration<WiredNetworkAdapter>
{
    public void Configure(EntityTypeBuilder<WiredNetworkAdapter> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.MaxSpeedMbps)
            .IsRequired();

        builder.HasOne(w => w.Manufacturer)
            .WithMany(m => m.WiredNetworkAdapters)
            .HasForeignKey(w => w.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}