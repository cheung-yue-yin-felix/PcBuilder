using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class GpuConfiguration : IEntityTypeConfiguration<Gpu>
{
    public void Configure(EntityTypeBuilder<Gpu> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(gpu => gpu.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne<GpuSeries>()
            .WithMany(gpuSeries => gpuSeries.Gpus)
            .HasForeignKey(gpu => gpu.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Manufacturer>()
            .WithMany(manufacturer => manufacturer.Gpus)
            .HasForeignKey(gpu => gpu.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}