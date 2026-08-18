using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWirelessAdapterM2AndPcieFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<PcieSlotType>(
                name: "PcieSlotType",
                table: "WirelessNetworkAdapters",
                type: "pcie_slot_type",
                nullable: true);

            migrationBuilder.AddColumn<M2Key>(
                name: "Key",
                table: "WirelessNetworkAdapters",
                type: "m2_key",
                nullable: true);

            migrationBuilder.AddColumn<M2FormFactor>(
                name: "M2FormFactor",
                table: "WirelessNetworkAdapters",
                type: "m2_form_factor",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "WirelessNetworkAdapters"
                SET "Key" = 'e'::m2_key,
                    "M2FormFactor" = 'm22230'::m2_form_factor
                WHERE "HostInterface" = 'm2';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PcieSlotType",
                table: "WirelessNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "WirelessNetworkAdapters");

            migrationBuilder.DropColumn(
                name: "M2FormFactor",
                table: "WirelessNetworkAdapters");
        }
    }
}
