using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuCoolerSocketConfiguration : IEntityTypeConfiguration<CpuCoolerSocket>
{
    public void Configure(EntityTypeBuilder<CpuCoolerSocket> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.HasOne(c => c.CpuCooler)
            .WithMany()
            .HasForeignKey(c => c.CpuCoolerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Socket)
            .WithMany()
            .HasForeignKey(c => c.SocketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}