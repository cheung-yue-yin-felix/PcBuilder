using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Infrastructure.Persistence.Configurations;

internal sealed class MotherboardM2Configuration : IEntityTypeConfiguration<MotherboardM2>
{
    public void Configure(EntityTypeBuilder<MotherboardM2> builder)
    {
        builder.ConfigureGuidBaseEntity();
        builder.Property(m2 => m2.FormFactors)
            .HasColumnType("m2_form_factor[]");
    }
}