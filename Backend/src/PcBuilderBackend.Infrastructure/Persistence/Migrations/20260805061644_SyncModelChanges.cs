using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLowProfile",
                table: "GraphicsCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MotherboardMaxHeightMm",
                table: "Chassis",
                type: "numeric(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MotherboardMaxWidthMm",
                table: "Chassis",
                type: "numeric(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLowProfile",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "MotherboardMaxHeightMm",
                table: "Chassis");

            migrationBuilder.DropColumn(
                name: "MotherboardMaxWidthMm",
                table: "Chassis");
        }
    }
}
