using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class SocketConfiguration : IEntityTypeConfiguration<Socket>
{
    public void Configure(EntityTypeBuilder<Socket> builder)
    {
        builder.ConfigureGuidBaseEntity();

        builder.Property(socket => socket.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(socket => socket.Manufacturer)
            .WithMany(manufacturer => manufacturer.Sockets)
            .HasForeignKey(socket => socket.ManufacturerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}