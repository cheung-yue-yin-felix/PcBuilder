using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class PsuConfiguration : IEntityTypeConfiguration<Psu>
{
    public void Configure(EntityTypeBuilder<Psu> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(psu => psu.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(psu => psu.LengthMm)
            .HasPrecision(6, 2);

        builder.Property(psu => psu.WidthMm)
            .HasPrecision(6, 2);

        builder.Property(psu => psu.HeightMm)
            .HasPrecision(6, 2);

        builder.HasOne(psu => psu.Manufacturer)
            .WithMany(manufacturer => manufacturer.Psus)
            .HasForeignKey(psu => psu.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(psu => psu.Cables)
            .WithOne()
            .HasForeignKey(cable => cable.PsuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}