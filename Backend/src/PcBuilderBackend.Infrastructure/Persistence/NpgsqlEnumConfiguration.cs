using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Infrastructure.Persistence;

internal static class NpgsqlEnumConfiguration
{
    public static void MapDomainEnums(this NpgsqlDbContextOptionsBuilder options)
    {
        options.MapEnum<DdrGeneration>("ddr_generation");
        options.MapEnum<MbFormFactor>("mb_form_factor");
        options.MapEnum<PcieGeneration>("pcie_generation");
        options.MapEnum<PcieSlotLane>("pcie_slot_lane");
        options.MapEnum<PcieSlotType>("pcie_slot_type");
        options.MapEnum<PsuCableType>("psu_cable_type");
        options.MapEnum<PsuFormFactor>("psu_form_factor");
        options.MapEnum<PsuModularity>("psu_modularity");
        options.MapEnum<RamFormFactor>("ram_form_factor");
        options.MapEnum<M2FormFactor>("m2_form_factor");
    }

    public static void ConfigurePostgresEnums(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<DdrGeneration>(name: "ddr_generation");
        modelBuilder.HasPostgresEnum<MbFormFactor>(name: "mb_form_factor");
        modelBuilder.HasPostgresEnum<PcieGeneration>(name: "pcie_generation");
        modelBuilder.HasPostgresEnum<PcieSlotLane>(name: "pcie_slot_lane");
        modelBuilder.HasPostgresEnum<PcieSlotType>(name: "pcie_slot_type");
        modelBuilder.HasPostgresEnum<PsuCableType>(name: "psu_cable_type");
        modelBuilder.HasPostgresEnum<PsuFormFactor>(name: "psu_form_factor");
        modelBuilder.HasPostgresEnum<PsuModularity>(name: "psu_modularity");
        modelBuilder.HasPostgresEnum<RamFormFactor>(name: "ram_form_factor");
        modelBuilder.HasPostgresEnum<M2FormFactor>(name: "m2_form_factor");
    }
}