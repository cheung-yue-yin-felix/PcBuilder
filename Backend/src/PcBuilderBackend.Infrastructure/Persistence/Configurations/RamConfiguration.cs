using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class RamConfiguration : IEntityTypeConfiguration<Ram>
{
    public void Configure(EntityTypeBuilder<Ram> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(ram => ram.HeightMm).HasPrecision(6, 2);

        builder.HasOne(ram => ram.Manufacturer)
            .WithMany(manufacturer => manufacturer.Rams)
            .HasForeignKey(ram => ram.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}