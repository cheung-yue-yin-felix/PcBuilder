using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class GraphicsCardConfiguration : IEntityTypeConfiguration<GraphicsCard>
{
    public void Configure(EntityTypeBuilder<GraphicsCard> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(graphicsCard => graphicsCard.LengthMm)
            .HasPrecision(6, 2);

        builder.Property(graphicsCard => graphicsCard.WidthMm)
            .HasPrecision(6, 2);

        builder.Property(graphicsCard => graphicsCard.HeightMm)
            .HasPrecision(6, 2);

        builder.Property(graphicsCard => graphicsCard.PowerConsumptionWatts)
            .HasPrecision(7, 2);

        builder.HasOne(graphicsCard => graphicsCard.Manufacturer)
            .WithMany(manufacturer => manufacturer.GraphicsCards)
            .HasForeignKey(graphicsCard => graphicsCard.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(graphicsCard => graphicsCard.Gpu)
            .WithMany()
            .HasForeignKey(graphicsCard => graphicsCard.GpuId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}