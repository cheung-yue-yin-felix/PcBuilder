using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WirelessNetworkAdapters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WiredNetworkAdapters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StorageDrives",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Sockets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Psus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PsuCables",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Motherboards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MotherboardPcieSlots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MotherboardM2Slots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Manufacturers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GraphicsCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GraphicsCardPowerConnectors",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Gpus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Cpus",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CpuCoolerSockets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CpuCoolers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Chipsets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisRadiators",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisPcieSlots",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisFans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisFanMounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisFanMountOptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChassisDriveBays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Chassis",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WirelessNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WiredNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StorageDrives");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Sockets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Psus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PsuCables");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MotherboardPcieSlots");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MotherboardM2Slots");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GraphicsCardPowerConnectors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CpuCoolerSockets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CpuCoolers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Chipsets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisRadiators");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisPcieSlots");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisFans");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisFanMounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisFanMountOptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ChassisDriveBays");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Chassis");
        }
    }
}
