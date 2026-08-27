using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class GpuSeriesConfiguration : IEntityTypeConfiguration<GpuSeries>
{
    public void Configure(EntityTypeBuilder<GpuSeries> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(gs => gs.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(gs => gs.Gpus)
            .WithOne(g => g.Series)
            .HasForeignKey(g => g.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Manufacturer>(gs => gs.Manufacturer)
            .WithMany(m => m.GpuSeries)
            .HasForeignKey(gs => gs.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
