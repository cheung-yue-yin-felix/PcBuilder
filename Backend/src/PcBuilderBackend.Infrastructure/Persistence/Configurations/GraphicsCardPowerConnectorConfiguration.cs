using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class GraphicsCardPowerConnectorConfiguration : IEntityTypeConfiguration<GraphicsCardPowerConnector>
{
    public void Configure(EntityTypeBuilder<GraphicsCardPowerConnector> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(g => g.ConnectorCount)
            .IsRequired();

        builder.HasOne<GraphicsCard>()
            .WithMany(gc => gc.PowerConnectors)
            .HasForeignKey(g => g.GraphicsCardId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => new { g.GraphicsCardId, g.PsuCableType })
            .IsUnique();
    }
}