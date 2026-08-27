using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportedMemorySpeeds",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "SupportedMbFormFactors",
                table: "Chassis");

            migrationBuilder.DropColumn(
                name: "SupportedPsuFormFactors",
                table: "Chassis");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:bluetooth_version", "v5point0,v5point1,v5point2,v5point3,v5point4")
                .Annotation("Npgsql:Enum:cpu_cooler_type", "air,water")
                .Annotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .Annotation("Npgsql:Enum:drive_bay_form_factor", "inch25,inch35,inch525")
                .Annotation("Npgsql:Enum:fan_diameter_mm", "mm80,mm92,mm120,mm140,mm200")
                .Annotation("Npgsql:Enum:fan_mount_location", "front,top,bottom,rear,sides")
                .Annotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .Annotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .Annotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .Annotation("Npgsql:Enum:pcie_orientation", "vertical,horizontal")
                .Annotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .Annotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .Annotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .Annotation("Npgsql:Enum:radiator_length", "mm120,mm140,mm240,mm280,mm360,mm420")
                .Annotation("Npgsql:Enum:radiator_mount_location", "front,top,bottom,rear,sides")
                .Annotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .Annotation("Npgsql:Enum:ram_rank", "single_rank,dual_rank")
                .Annotation("Npgsql:Enum:storage_form_factor", "m22230,m22242,m22260,m22280,m222110,sata25,sata35")
                .Annotation("Npgsql:Enum:storage_interface", "sata,nvme")
                .Annotation("Npgsql:Enum:storage_media", "hdd,ssd")
                .Annotation("Npgsql:Enum:wifi_standard", "wifi4,wifi5,wifi6,wifi6e,wifi7")
                .Annotation("Npgsql:Enum:wired_host_interface", "pcie,usb")
                .Annotation("Npgsql:Enum:wireless_host_interface", "m2,pcie,usb")
                .OldAnnotation("Npgsql:Enum:bluetooth_version", "v5point0,v5point1,v5point2,v5point3,v5point4")
                .OldAnnotation("Npgsql:Enum:cpu_cooler_type", "air,water")
                .OldAnnotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .OldAnnotation("Npgsql:Enum:drive_bay_form_factor", "inch25,inch35,inch525")
                .OldAnnotation("Npgsql:Enum:fan_diameter_mm", "mm80,mm92,mm120,mm140,mm200")
                .OldAnnotation("Npgsql:Enum:fan_mount_location", "front,top,bottom,rear,sides")
                .OldAnnotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .OldAnnotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .OldAnnotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .OldAnnotation("Npgsql:Enum:pcie_orientation", "vertical,horizontal")
                .OldAnnotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .OldAnnotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .OldAnnotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .OldAnnotation("Npgsql:Enum:radiator_length", "mm120,mm140,mm240,mm280,mm360,mm420")
                .OldAnnotation("Npgsql:Enum:radiator_mount_location", "front,top,bottom,rear,sides")
                .OldAnnotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .OldAnnotation("Npgsql:Enum:storage_form_factor", "m22230,m22242,m22260,m22280,m222110,sata25,sata35")
                .OldAnnotation("Npgsql:Enum:storage_interface", "sata,nvme")
                .OldAnnotation("Npgsql:Enum:storage_media", "hdd,ssd")
                .OldAnnotation("Npgsql:Enum:wifi_standard", "wifi4,wifi5,wifi6,wifi6e,wifi7")
                .OldAnnotation("Npgsql:Enum:wired_host_interface", "pcie,usb")
                .OldAnnotation("Npgsql:Enum:wireless_host_interface", "m2,pcie,usb");

            migrationBuilder.AddColumn<RamRank>(
                name: "RamRank",
                table: "Rams",
                type: "ram_rank",
                nullable: false,
                defaultValue: RamRank.SingleRank);

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Gpus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Cpus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ChassisMbFormFactor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    MbFormFactor = table.Column<MbFormFactor>(type: "mb_form_factor", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisMbFormFactor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisMbFormFactor_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChassisPsuFormFactor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    PsuFormFactor = table.Column<PsuFormFactor>(type: "psu_form_factor", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChassisPsuFormFactor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChassisPsuFormFactor_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuRamMaxSpeed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    RamModuleCount = table.Column<int>(type: "integer", nullable: false),
                    RamRank = table.Column<RamRank>(type: "ram_rank", nullable: false),
                    MaxSpeedMts = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuRamMaxSpeed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuRamMaxSpeed_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CpuSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SocketId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuSeries_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CpuSeries_Sockets_SocketId",
                        column: x => x.SocketId,
                        principalTable: "Sockets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GpuSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GpuSeries_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_SeriesId",
                table: "Gpus",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_SeriesId",
                table: "Cpus",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisMbFormFactor_ChassisId",
                table: "ChassisMbFormFactor",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPsuFormFactor_ChassisId",
                table: "ChassisPsuFormFactor",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuRamMaxSpeed_CpuId",
                table: "CpuRamMaxSpeed",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuSeries_ManufacturerId",
                table: "CpuSeries",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuSeries_SocketId",
                table: "CpuSeries",
                column: "SocketId");

            migrationBuilder.CreateIndex(
                name: "IX_GpuSeries_ManufacturerId",
                table: "GpuSeries",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cpus_CpuSeries_SeriesId",
                table: "Cpus",
                column: "SeriesId",
                principalTable: "CpuSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_GpuSeries_SeriesId",
                table: "Gpus",
                column: "SeriesId",
                principalTable: "GpuSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cpus_CpuSeries_SeriesId",
                table: "Cpus");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_GpuSeries_SeriesId",
                table: "Gpus");

            migrationBuilder.DropTable(
                name: "ChassisMbFormFactor");

            migrationBuilder.DropTable(
                name: "ChassisPsuFormFactor");

            migrationBuilder.DropTable(
                name: "CpuRamMaxSpeed");

            migrationBuilder.DropTable(
                name: "CpuSeries");

            migrationBuilder.DropTable(
                name: "GpuSeries");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_SeriesId",
                table: "Gpus");

            migrationBuilder.DropIndex(
                name: "IX_Cpus_SeriesId",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "RamRank",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Cpus");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:bluetooth_version", "v5point0,v5point1,v5point2,v5point3,v5point4")
                .Annotation("Npgsql:Enum:cpu_cooler_type", "air,water")
                .Annotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .Annotation("Npgsql:Enum:drive_bay_form_factor", "inch25,inch35,inch525")
                .Annotation("Npgsql:Enum:fan_diameter_mm", "mm80,mm92,mm120,mm140,mm200")
                .Annotation("Npgsql:Enum:fan_mount_location", "front,top,bottom,rear,sides")
                .Annotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .Annotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .Annotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .Annotation("Npgsql:Enum:pcie_orientation", "vertical,horizontal")
                .Annotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .Annotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .Annotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .Annotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .Annotation("Npgsql:Enum:radiator_length", "mm120,mm140,mm240,mm280,mm360,mm420")
                .Annotation("Npgsql:Enum:radiator_mount_location", "front,top,bottom,rear,sides")
                .Annotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .Annotation("Npgsql:Enum:storage_form_factor", "m22230,m22242,m22260,m22280,m222110,sata25,sata35")
                .Annotation("Npgsql:Enum:storage_interface", "sata,nvme")
                .Annotation("Npgsql:Enum:storage_media", "hdd,ssd")
                .Annotation("Npgsql:Enum:wifi_standard", "wifi4,wifi5,wifi6,wifi6e,wifi7")
                .Annotation("Npgsql:Enum:wired_host_interface", "pcie,usb")
                .Annotation("Npgsql:Enum:wireless_host_interface", "m2,pcie,usb")
                .OldAnnotation("Npgsql:Enum:bluetooth_version", "v5point0,v5point1,v5point2,v5point3,v5point4")
                .OldAnnotation("Npgsql:Enum:cpu_cooler_type", "air,water")
                .OldAnnotation("Npgsql:Enum:ddr_generation", "ddr4,ddr5")
                .OldAnnotation("Npgsql:Enum:drive_bay_form_factor", "inch25,inch35,inch525")
                .OldAnnotation("Npgsql:Enum:fan_diameter_mm", "mm80,mm92,mm120,mm140,mm200")
                .OldAnnotation("Npgsql:Enum:fan_mount_location", "front,top,bottom,rear,sides")
                .OldAnnotation("Npgsql:Enum:m2_form_factor", "m22230,m22242,m22260,m22280,m222110")
                .OldAnnotation("Npgsql:Enum:mb_form_factor", "mitx,matx,atx,eatx")
                .OldAnnotation("Npgsql:Enum:pcie_generation", "gen3,gen4,gen5,gen6")
                .OldAnnotation("Npgsql:Enum:pcie_orientation", "vertical,horizontal")
                .OldAnnotation("Npgsql:Enum:pcie_slot_lane", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:pcie_slot_type", "x1,x4,x8,x16")
                .OldAnnotation("Npgsql:Enum:psu_cable_type", "motherboard24pin,cpu4plus4pin,pcie6plus2pin,pcie12v_high_power,pcie12v2x6,sata,molex,floppy")
                .OldAnnotation("Npgsql:Enum:psu_form_factor", "flex_atx,tfx,sfx,sfx_l,atx")
                .OldAnnotation("Npgsql:Enum:psu_modularity", "non_modular,semi_modular,full_modular")
                .OldAnnotation("Npgsql:Enum:radiator_length", "mm120,mm140,mm240,mm280,mm360,mm420")
                .OldAnnotation("Npgsql:Enum:radiator_mount_location", "front,top,bottom,rear,sides")
                .OldAnnotation("Npgsql:Enum:ram_form_factor", "u_dimm,so_dimm")
                .OldAnnotation("Npgsql:Enum:ram_rank", "single_rank,dual_rank")
                .OldAnnotation("Npgsql:Enum:storage_form_factor", "m22230,m22242,m22260,m22280,m222110,sata25,sata35")
                .OldAnnotation("Npgsql:Enum:storage_interface", "sata,nvme")
                .OldAnnotation("Npgsql:Enum:storage_media", "hdd,ssd")
                .OldAnnotation("Npgsql:Enum:wifi_standard", "wifi4,wifi5,wifi6,wifi6e,wifi7")
                .OldAnnotation("Npgsql:Enum:wired_host_interface", "pcie,usb")
                .OldAnnotation("Npgsql:Enum:wireless_host_interface", "m2,pcie,usb");

            migrationBuilder.AddColumn<List<int>>(
                name: "SupportedMemorySpeeds",
                table: "Motherboards",
                type: "integer[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Gpus",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Cpus",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<MbFormFactor>>(
                name: "SupportedMbFormFactors",
                table: "Chassis",
                type: "mb_form_factor[]",
                nullable: false);

            migrationBuilder.AddColumn<List<PsuFormFactor>>(
                name: "SupportedPsuFormFactors",
                table: "Chassis",
                type: "psu_form_factor[]",
                nullable: false);
        }
    }
}
