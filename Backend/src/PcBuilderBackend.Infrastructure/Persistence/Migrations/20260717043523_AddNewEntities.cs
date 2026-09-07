using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalMemorySize",
                table: "Rams",
                newName: "TotalMemorySizeGb");

            migrationBuilder.RenameColumn(
                name: "VideoMemory",
                table: "GraphicsCards",
                newName: "VideoMemoryGb");

            migrationBuilder.RenameColumn(
                name: "PcieSlots",
                table: "GraphicsCards",
                newName: "PcieSlotsUsed");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .Annotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .Annotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .Annotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .Annotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .Annotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .Annotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .Annotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .OldAnnotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .OldAnnotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .OldAnnotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .OldAnnotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .OldAnnotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .OldAnnotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .OldAnnotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Rams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "HeightMm",
                table: "Rams",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Rams",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Psus",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<bool>(
                name: "BluetoothEnabled",
                table: "Motherboards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "EpsConnectors",
                table: "Motherboards",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Motherboards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "WifiEnabled",
                table: "Motherboards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<List<M2FormFactor>>(
                name: "FormFactors",
                table: "MotherboardM2Slots",
                type: "m2_form_factor[]",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "GraphicsCards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "GraphicsCards",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Gpus",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DdrGeneration>(
                name: "DdrGeneration",
                table: "Cpus",
                type: "ddr_generation",
                nullable: false,
                defaultValue: DdrGeneration.Ddr4);

            migrationBuilder.AddColumn<bool>(
                name: "IncludedStockCooler",
                table: "Cpus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IntegratedGraphics",
                table: "Cpus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Cpus",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ThermalDesignPower",
                table: "Cpus",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Chipsets",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.AddColumn<Guid>(
                name: "SocketId",
                table: "Chipsets",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateTable(
                name: "Chassis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SupportedMbFormFactors = table.Column<List<MbFormFactor>>(type: "mb_form_factor[]", nullable: false),
                    SupportedPsuFormFactors = table.Column<List<PsuFormFactor>>(type: "psu_form_factor[]", nullable: false),
                    LengthMm = table.Column<double>(type: "double precision", nullable: false),
                    WidthMm = table.Column<double>(type: "double precision", nullable: false),
                    HeightMm = table.Column<double>(type: "double precision", nullable: false),
                    MaxCpuCoolerHeightMm = table.Column<double>(type: "double precision", nullable: false),
                    MaxGraphicsCardLengthMm = table.Column<double>(type: "double precision", nullable: false),
                    MaxPsuLengthMm = table.Column<double>(type: "double precision", nullable: false),
                    ManufacturerId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chassis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chassis_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Chassis_Manufacturers_ManufacturerId1",
                        column: x => x.ManufacturerId1,
                        principalTable: "Manufacturers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChassisFans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DiameterMm = table.Column<int>(type: "integer", nullable: false),
                    FansCountPerPack = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisFans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisFans_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuCoolers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    MaxTdp = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CoolerHeightMm = table.Column<double>(type: "double precision", nullable: true),
                    MaxRamHeightMm = table.Column<double>(type: "double precision", nullable: true),
                    RadiatorLength = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuCoolers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuCoolers_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GraphicsCardPowerConnectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GraphicsCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    PsuCableType = table.Column<PsuCableType>(type: "psu_cable_type", nullable: false),
                    ConnectorCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicsCardPowerConnectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                        column: x => x.GraphicsCardId,
                        principalTable: "GraphicsCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorageDrives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Media = table.Column<int>(type: "integer", nullable: false),
                    Interface = table.Column<int>(type: "integer", nullable: false),
                    FormFactor = table.Column<int>(type: "integer", nullable: false),
                    CapacityGb = table.Column<int>(type: "integer", nullable: false),
                    PcieGeneration = table.Column<PcieGeneration>(type: "pcie_generation", nullable: true),
                    Rpm = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageDrives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageDrives_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WiredNetworkAdapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    HostInterface = table.Column<int>(type: "integer", nullable: false),
                    MaxSpeedMbps = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WiredNetworkAdapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WiredNetworkAdapters_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WirelessNetworkAdapters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WifiStandard = table.Column<int>(type: "integer", nullable: false),
                    BluetoothVersion = table.Column<int>(type: "integer", nullable: true),
                    HostInterface = table.Column<int>(type: "integer", nullable: false),
                    MaxSpeedMbps = table.Column<int>(type: "integer", nullable: false),
                    MaxSpeedMbps5G = table.Column<int>(type: "integer", nullable: true),
                    MaxSpeedMbps6G = table.Column<int>(type: "integer", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WirelessNetworkAdapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WirelessNetworkAdapters_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisDriveBays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    DriveBayFormFactor = table.Column<int>(type: "integer", nullable: false),
                    BayCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisDriveBays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisDriveBays_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisFanMounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<int>(type: "integer", nullable: false),
                    SingleDiameterOnly = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisFanMounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisFanMounts_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisPcieSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    LowProfileSlots = table.Column<bool>(type: "boolean", nullable: false),
                    SlotCount = table.Column<int>(type: "integer", nullable: false),
                    Orientation = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisPcieSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisPcieSlots_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisRadiators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    Length = table.Column<int>(type: "integer", nullable: false),
                    MountLocation = table.Column<int>(type: "integer", nullable: false),
                    RadiatorCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisRadiators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisRadiators_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuCoolerSockets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuCoolerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SocketId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuCoolerSockets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuCoolerSockets_CpuCoolers_CpuCoolerId",
                        column: x => x.CpuCoolerId,
                        principalTable: "CpuCoolers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CpuCoolerSockets_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisFanMountOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisFanMountId = table.Column<Guid>(type: "uuid", nullable: false),
                    Diameter = table.Column<int>(type: "integer", nullable: false),
                    SlotCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisFanMountOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisFanMountOptions_ChassisFanMounts_ChassisFanMountId",
                        column: x => x.ChassisFanMountId,
                        principalTable: "ChassisFanMounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rams_ManufacturerId1",
                table: "Rams",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Psus_ManufacturerId1",
                table: "Psus",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_ManufacturerId1",
                table: "Motherboards",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCards_ManufacturerId1",
                table: "GraphicsCards",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Chipsets_ManufacturerId1",
                table: "Chipsets",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Chipsets_SocketId",
                table: "Chipsets",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_Chassis_ManufacturerId",
                table: "Chassis",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Chassis_ManufacturerId1",
                table: "Chassis",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisDriveBays_ChassisId",
                table: "ChassisDriveBays",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMountOptions_ChassisFanMountId",
                table: "ChassisFanMountOptions",
                column: "ChassisFanMountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMounts_ChassisId",
                table: "ChassisFanMounts",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFans_ManufacturerId",
                table: "ChassisFans",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPcieSlots_ChassisId",
                table: "ChassisPcieSlots",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisRadiators_ChassisId",
                table: "ChassisRadiators",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoolers_ManufacturerId",
                table: "CpuCoolers",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoolerSockets_CpuCoolerId",
                table: "CpuCoolerSockets",
                column: "CpuCoolerId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoolerSockets_SocketId",
                table: "CpuCoolerSockets",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId",
                table: "GraphicsCardPowerConnectors",
                column: "GraphicsCardId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageDrives_ManufacturerId",
                table: "StorageDrives",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_WiredNetworkAdapters_ManufacturerId",
                table: "WiredNetworkAdapters",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_WirelessNetworkAdapters_ManufacturerId",
                table: "WirelessNetworkAdapters",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chipsets_Manufacturers_ManufacturerId1",
                table: "Chipsets",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphicsCards_Manufacturers_ManufacturerId1",
                table: "GraphicsCards",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Motherboards_Manufacturers_ManufacturerId1",
                table: "Motherboards",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Psus_Manufacturers_ManufacturerId1",
                table: "Psus",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rams_Manufacturers_ManufacturerId1",
                table: "Rams",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chipsets_Manufacturers_ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphicsCards_Manufacturers_ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Motherboards_Manufacturers_ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Psus_Manufacturers_ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropForeignKey(
                name: "FK_Rams_Manufacturers_ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropTable(
                name: "ChassisDriveBays");

            migrationBuilder.DropTable(
                name: "ChassisFanMountOptions");

            migrationBuilder.DropTable(
                name: "ChassisFans");

            migrationBuilder.DropTable(
                name: "ChassisPcieSlots");

            migrationBuilder.DropTable(
                name: "ChassisRadiators");

            migrationBuilder.DropTable(
                name: "CpuCoolerSockets");

            migrationBuilder.DropTable(
                name: "GraphicsCardPowerConnectors");

            migrationBuilder.DropTable(
                name: "StorageDrives");

            migrationBuilder.DropTable(
                name: "WiredNetworkAdapters");

            migrationBuilder.DropTable(
                name: "WirelessNetworkAdapters");

            migrationBuilder.DropTable(
                name: "ChassisFanMounts");

            migrationBuilder.DropTable(
                name: "CpuCoolers");

            migrationBuilder.DropTable(
                name: "Chassis");

            migrationBuilder.DropIndex(
                name: "IX_Rams_ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropIndex(
                name: "IX_Psus_ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropIndex(
                name: "IX_Motherboards_ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropIndex(
                name: "IX_GraphicsCards_ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropIndex(
                name: "IX_Chipsets_ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropIndex(
                name: "IX_Chipsets_SocketId",
                table: "Chipsets");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "HeightMm",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropColumn(
                name: "BluetoothEnabled",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "EpsConnectors",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "WifiEnabled",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "FormFactors",
                table: "MotherboardM2Slots");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "DdrGeneration",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "IncludedStockCooler",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "IntegratedGraphics",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "ThermalDesignPower",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropColumn(
                name: "SocketId",
                table: "Chipsets");

            migrationBuilder.RenameColumn(
                name: "TotalMemorySizeGb",
                table: "Rams",
                newName: "TotalMemorySize");

            migrationBuilder.RenameColumn(
                name: "VideoMemoryGb",
                table: "GraphicsCards",
                newName: "VideoMemory");

            migrationBuilder.RenameColumn(
                name: "PcieSlotsUsed",
                table: "GraphicsCards",
                newName: "PcieSlots");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .Annotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .Annotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .Annotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .Annotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .Annotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .Annotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .OldAnnotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .OldAnnotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .OldAnnotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .OldAnnotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .OldAnnotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .OldAnnotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .OldAnnotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .OldAnnotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm");
        }
    }
}
