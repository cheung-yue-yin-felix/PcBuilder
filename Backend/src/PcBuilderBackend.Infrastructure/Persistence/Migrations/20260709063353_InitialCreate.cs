using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("""
                DROP TYPE IF EXISTS psu_cable_type CASCADE;
                DROP TYPE IF EXISTS psu_modularity CASCADE;
                DROP TYPE IF EXISTS psu_form_factor CASCADE;
                DROP TYPE IF EXISTS ram_form_factor CASCADE;
                DROP TYPE IF EXISTS pcie_slot_type CASCADE;
                DROP TYPE IF EXISTS pcie_slot_lane CASCADE;
                DROP TYPE IF EXISTS pcie_generation CASCADE;
                DROP TYPE IF EXISTS mb_form_factor CASCADE;
                DROP TYPE IF EXISTS ddr_generation CASCADE;
                """);
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .Annotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .Annotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .Annotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .Annotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .Annotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .Annotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm");

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chipsets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chipsets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chipsets_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Gpus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Series = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gpus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gpus_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Psus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Wattage = table.Column<int>(type: "integer", nullable: false),
                    Modularity = table.Column<PsuModularity>(type: "psu_modularity", nullable: false),
                    FormFactor = table.Column<PsuFormFactor>(type: "psu_form_factor", nullable: false),
                    LengthMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    WidthMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    HeightMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Psus_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DdrGeneration = table.Column<DdrGeneration>(type: "ddr_generation", nullable: false),
                    RamFormFactor = table.Column<RamFormFactor>(type: "ram_form_factor", nullable: false),
                    MemorySizePerStickGb = table.Column<int>(type: "integer", nullable: false),
                    TotalMemorySize = table.Column<int>(type: "integer", nullable: false),
                    ModulesCount = table.Column<int>(type: "integer", nullable: false),
                    MaxMemorySpeedMts = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rams_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sockets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sockets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sockets_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GraphicsCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    VideoMemory = table.Column<int>(type: "integer", nullable: false),
                    PcieSlots = table.Column<int>(type: "integer", nullable: false),
                    PcieGeneration = table.Column<PcieGeneration>(type: "pcie_generation", nullable: false),
                    LengthMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    WidthMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    HeightMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    PowerConsumptionWatts = table.Column<decimal>(type: "numeric(7,2)", precision: 7, scale: 2, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicsCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphicsCards_Gpus_GpuId",
                        column: x => x.GpuId,
                        principalTable: "Gpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GraphicsCards_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PsuCables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PsuId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<PsuCableType>(type: "psu_cable_type", nullable: false),
                    CablesCount = table.Column<int>(type: "integer", nullable: false),
                    ConnectorsCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PsuCables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PsuCables_Psus_PsuId",
                        column: x => x.PsuId,
                        principalTable: "Psus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cpus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SocketId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxMemoryGb = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cpus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cpus_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cpus_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Motherboards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SocketId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChipsetId = table.Column<Guid>(type: "uuid", nullable: false),
                    RamSlots = table.Column<int>(type: "integer", nullable: false),
                    MaxMemoryGb = table.Column<int>(type: "integer", nullable: false),
                    MaxDimmSizeGb = table.Column<int>(type: "integer", nullable: false),
                    SataPorts = table.Column<int>(type: "integer", nullable: false),
                    FanConnectors = table.Column<int>(type: "integer", nullable: false),
                    WidthMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    HeightMm = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    DdrGeneration = table.Column<DdrGeneration>(type: "ddr_generation", nullable: false),
                    RamFormFactor = table.Column<RamFormFactor>(type: "ram_form_factor", nullable: false),
                    FormFactor = table.Column<MbFormFactor>(type: "mb_form_factor", nullable: false),
                    SupportedMemorySpeeds = table.Column<List<int>>(type: "integer[]", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motherboards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Motherboards_Chipsets_ChipsetId",
                        column: x => x.ChipsetId,
                        principalTable: "Chipsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motherboards_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Motherboards_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardM2Slots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    PcieGeneration = table.Column<PcieGeneration>(type: "pcie_generation", nullable: false),
                    SlotCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardM2Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherboardM2Slots_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MotherboardPcieSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotType = table.Column<PcieSlotType>(type: "pcie_slot_type", nullable: false),
                    SlotLanes = table.Column<PcieSlotLane>(type: "pcie_slot_lane", nullable: false),
                    Generation = table.Column<PcieGeneration>(type: "pcie_generation", nullable: false),
                    SlotCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardPcieSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherboardPcieSlots_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chipsets_ManufacturerId",
                table: "Chipsets",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_ManufacturerId",
                table: "Cpus",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_SocketId",
                table: "Cpus",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ManufacturerId",
                table: "Gpus",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCards_GpuId",
                table: "GraphicsCards",
                column: "GpuId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCards_ManufacturerId",
                table: "GraphicsCards",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId",
                table: "MotherboardM2Slots",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPcieSlots_MotherboardId",
                table: "MotherboardPcieSlots",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_ChipsetId",
                table: "Motherboards",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_ManufacturerId",
                table: "Motherboards",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_SocketId",
                table: "Motherboards",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_PsuCables_PsuId",
                table: "PsuCables",
                column: "PsuId");

            migrationBuilder.CreateIndex(
                name: "IX_Psus_ManufacturerId",
                table: "Psus",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Rams_ManufacturerId",
                table: "Rams",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sockets_ManufacturerId",
                table: "Sockets",
                column: "ManufacturerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cpus");

            migrationBuilder.DropTable(
                name: "GraphicsCards");

            migrationBuilder.DropTable(
                name: "MotherboardM2Slots");

            migrationBuilder.DropTable(
                name: "MotherboardPcieSlots");

            migrationBuilder.DropTable(
                name: "PsuCables");

            migrationBuilder.DropTable(
                name: "Rams");

            migrationBuilder.DropTable(
                name: "Gpus");

            migrationBuilder.DropTable(
                name: "Motherboards");

            migrationBuilder.DropTable(
                name: "Psus");

            migrationBuilder.DropTable(
                name: "Chipsets");

            migrationBuilder.DropTable(
                name: "Sockets");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.Sql("""
                DROP TYPE IF EXISTS psu_cable_type CASCADE;
                DROP TYPE IF EXISTS psu_modularity CASCADE;
                DROP TYPE IF EXISTS psu_form_factor CASCADE;
                DROP TYPE IF EXISTS ram_form_factor CASCADE;
                DROP TYPE IF EXISTS pcie_slot_type CASCADE;
                DROP TYPE IF EXISTS pcie_slot_lane CASCADE;
                DROP TYPE IF EXISTS pcie_generation CASCADE;
                DROP TYPE IF EXISTS mb_form_factor CASCADE;
                DROP TYPE IF EXISTS ddr_generation CASCADE;
                """);
        }
    }
}
