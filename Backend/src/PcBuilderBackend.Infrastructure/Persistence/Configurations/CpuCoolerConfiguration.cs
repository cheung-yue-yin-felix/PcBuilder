using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuCoolerConfiguration : IEntityTypeConfiguration<CpuCooler>
{
    public void Configure(EntityTypeBuilder<CpuCooler> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.MaxTdp)
            .IsRequired();

        builder.HasOne(c => c.Manufacturer)
            .WithMany(m => m.CpuCoolers)
            .HasForeignKey(c => c.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}