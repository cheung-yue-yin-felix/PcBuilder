using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMotherboardUsbAndWiredPcieSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM pg_enum e
                        JOIN pg_type t ON t.oid = e.enumtypid
                        WHERE t.typname = 'usb_version' AND e.enumlabel = 'usb32_gen1') THEN
                        ALTER TYPE usb_version RENAME VALUE 'usb32_gen1' TO 'usb32gen1';
                    END IF;
                    IF EXISTS (
                        SELECT 1
                        FROM pg_enum e
                        JOIN pg_type t ON t.oid = e.enumtypid
                        WHERE t.typname = 'usb_version' AND e.enumlabel = 'usb32_gen2') THEN
                        ALTER TYPE usb_version RENAME VALUE 'usb32_gen2' TO 'usb32gen2';
                    END IF;
                END $$;
                """);

            migrationBuilder.AddColumn<PcieSlotType>(
                name: "PcieSlotType",
                table: "WiredNetworkAdapters",
                type: "pcie_slot_type",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MotherboardUsbPorts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsbVersion = table.Column<UsbVersion>(type: "usb_version", nullable: false),
                    UsbType = table.Column<UsbType>(type: "usb_type", nullable: false),
                    PortCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardUsbPorts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherboardUsbPorts_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardUsbPorts_MotherboardId_UsbType_UsbVersion",
                table: "MotherboardUsbPorts",
                columns: new[] { "MotherboardId", "UsbType", "UsbVersion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherboardUsbPorts");

            migrationBuilder.DropColumn(
                name: "PcieSlotType",
                table: "WiredNetworkAdapters");
        }
    }
}
