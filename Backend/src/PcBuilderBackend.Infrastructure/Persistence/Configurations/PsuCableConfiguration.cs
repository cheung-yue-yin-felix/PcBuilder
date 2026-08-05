using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class PsuCableConfiguration : IEntityTypeConfiguration<PsuCable>
{
    public void Configure(EntityTypeBuilder<PsuCable> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasIndex(c => new { c.PsuId, c.Type })
            .IsUnique();
    }
}