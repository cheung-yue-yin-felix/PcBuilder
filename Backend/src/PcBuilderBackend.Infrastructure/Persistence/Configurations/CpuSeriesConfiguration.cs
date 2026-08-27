using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuSeriesConfiguration : IEntityTypeConfiguration<CpuSeries>
{
    public void Configure(EntityTypeBuilder<CpuSeries> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.ToTable("CpuSeries");

        builder.Property(cs => cs.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(cs => cs.Manufacturer)
            .WithMany(m => m.CpuSeries)
            .HasForeignKey(cs => cs.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cs => cs.Socket)
            .WithMany()
            .HasForeignKey(cs => cs.SocketId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(cs => cs.Cpus)
            .WithOne(c => c.Series)
            .HasForeignKey(c => c.SeriesId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(cs => cs.ManufacturerId);
        builder.HasIndex(cs => cs.SocketId);
    }
}
