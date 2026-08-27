using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChassisDriveBays_Chassis_ChassisId",
                table: "ChassisDriveBays");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFanMountOptions_ChassisFanMounts_ChassisFanMountId",
                table: "ChassisFanMountOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFanMounts_Chassis_ChassisId",
                table: "ChassisFanMounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFans_Manufacturers_ManufacturerId",
                table: "ChassisFans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisPcieSlots_Chassis_ChassisId",
                table: "ChassisPcieSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisRadiators_Chassis_ChassisId",
                table: "ChassisRadiators");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolers_Manufacturers_ManufacturerId",
                table: "CpuCoolers");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolerSockets_CpuCoolers_CpuCoolerId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolerSockets_Sockets_SocketId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                table: "GraphicsCardPowerConnectors");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageDrives_Manufacturers_ManufacturerId",
                table: "StorageDrives");

            migrationBuilder.DropForeignKey(
                name: "FK_WiredNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WiredNetworkAdapters");

            migrationBuilder.DropForeignKey(
                name: "FK_WirelessNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WirelessNetworkAdapters");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WirelessNetworkAdapters",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WiredNetworkAdapters",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StorageDrives",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CpuCoolers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChassisFans",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisDriveBays_Chassis_ChassisId",
                table: "ChassisDriveBays",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFanMountOptions_ChassisFanMounts_ChassisFanMountId",
                table: "ChassisFanMountOptions",
                column: "ChassisFanMountId",
                principalTable: "ChassisFanMounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFanMounts_Chassis_ChassisId",
                table: "ChassisFanMounts",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFans_Manufacturers_ManufacturerId",
                table: "ChassisFans",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisPcieSlots_Chassis_ChassisId",
                table: "ChassisPcieSlots",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisRadiators_Chassis_ChassisId",
                table: "ChassisRadiators",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolers_Manufacturers_ManufacturerId",
                table: "CpuCoolers",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolerSockets_CpuCoolers_CpuCoolerId",
                table: "CpuCoolerSockets",
                column: "CpuCoolerId",
                principalTable: "CpuCoolers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolerSockets_Sockets_SocketId",
                table: "CpuCoolerSockets",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                table: "GraphicsCardPowerConnectors",
                column: "GraphicsCardId",
                principalTable: "GraphicsCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageDrives_Manufacturers_ManufacturerId",
                table: "StorageDrives",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WiredNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WiredNetworkAdapters",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WirelessNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WirelessNetworkAdapters",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChassisDriveBays_Chassis_ChassisId",
                table: "ChassisDriveBays");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFanMountOptions_ChassisFanMounts_ChassisFanMountId",
                table: "ChassisFanMountOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFanMounts_Chassis_ChassisId",
                table: "ChassisFanMounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisFans_Manufacturers_ManufacturerId",
                table: "ChassisFans");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisPcieSlots_Chassis_ChassisId",
                table: "ChassisPcieSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_ChassisRadiators_Chassis_ChassisId",
                table: "ChassisRadiators");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolers_Manufacturers_ManufacturerId",
                table: "CpuCoolers");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolerSockets_CpuCoolers_CpuCoolerId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropForeignKey(
                name: "FK_CpuCoolerSockets_Sockets_SocketId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                table: "GraphicsCardPowerConnectors");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageDrives_Manufacturers_ManufacturerId",
                table: "StorageDrives");

            migrationBuilder.DropForeignKey(
                name: "FK_WiredNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WiredNetworkAdapters");

            migrationBuilder.DropForeignKey(
                name: "FK_WirelessNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WirelessNetworkAdapters");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WirelessNetworkAdapters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WiredNetworkAdapters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StorageDrives",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CpuCoolers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChassisFans",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisDriveBays_Chassis_ChassisId",
                table: "ChassisDriveBays",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFanMountOptions_ChassisFanMounts_ChassisFanMountId",
                table: "ChassisFanMountOptions",
                column: "ChassisFanMountId",
                principalTable: "ChassisFanMounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFanMounts_Chassis_ChassisId",
                table: "ChassisFanMounts",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisFans_Manufacturers_ManufacturerId",
                table: "ChassisFans",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisPcieSlots_Chassis_ChassisId",
                table: "ChassisPcieSlots",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChassisRadiators_Chassis_ChassisId",
                table: "ChassisRadiators",
                column: "ChassisId",
                principalTable: "Chassis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolers_Manufacturers_ManufacturerId",
                table: "CpuCoolers",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolerSockets_CpuCoolers_CpuCoolerId",
                table: "CpuCoolerSockets",
                column: "CpuCoolerId",
                principalTable: "CpuCoolers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuCoolerSockets_Sockets_SocketId",
                table: "CpuCoolerSockets",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                table: "GraphicsCardPowerConnectors",
                column: "GraphicsCardId",
                principalTable: "GraphicsCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageDrives_Manufacturers_ManufacturerId",
                table: "StorageDrives",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WiredNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WiredNetworkAdapters",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WirelessNetworkAdapters_Manufacturers_ManufacturerId",
                table: "WirelessNetworkAdapters",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
