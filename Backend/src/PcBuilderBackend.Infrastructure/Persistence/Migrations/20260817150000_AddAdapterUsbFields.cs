using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAdapterUsbFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'usb_version') THEN
                        CREATE TYPE usb_version AS ENUM ('usb20', 'usb32gen1', 'usb32gen2', 'usb4');
                    END IF;
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'usb_type') THEN
                        CREATE TYPE usb_type AS ENUM ('type_a', 'type_c');
                    END IF;
                END $$;
                """);

            migrationBuilder.AddColumn<UsbVersion>(
                name: "UsbVersion",
                table: "WirelessNetworkAdapters",
                type: "usb_version",
                nullable: true);

            migrationBuilder.AddColumn<UsbType>(
                name: "UsbType",
                table: "WirelessNetworkAdapters",
                type: "usb_type",
                nullable: true);

            migrationBuilder.AddColumn<UsbVersion>(
                name: "UsbVersion",
                table: "WiredNetworkAdapters",
                type: "usb_version",
                nullable: true);

            migrationBuilder.AddColumn<UsbType>(
                name: "UsbType",
                table: "WiredNetworkAdapters",
                type: "usb_type",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsbVersion",
                table: "WirelessNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "UsbType",
                table: "WirelessNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "UsbVersion",
                table: "WiredNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "UsbType",
                table: "WiredNetworkAdapters");

            migrationBuilder.Sql("DROP TYPE IF EXISTS usb_version;");
            migrationBuilder.Sql("DROP TYPE IF EXISTS usb_type;");
        }
    }
}
