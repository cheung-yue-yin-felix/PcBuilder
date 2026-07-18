using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class CpuConfiguration : IEntityTypeConfiguration<Cpu>
{
    public void Configure(EntityTypeBuilder<Cpu> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(cpu => cpu.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(cpu => cpu.Manufacturer)
            .WithMany(manufacturer => manufacturer.Cpus)
            .HasForeignKey(cpu => cpu.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cpu => cpu.Socket)
            .WithMany(socket => socket.Cpus)
            .HasForeignKey(cpu => cpu.SocketId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}