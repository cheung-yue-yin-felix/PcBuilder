using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropMotherboardM2KeyGenerationUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_Key_PcieGeneration",
                table: "MotherboardM2Slots");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId",
                table: "MotherboardM2Slots",
                column: "MotherboardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId",
                table: "MotherboardM2Slots");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_Key_PcieGeneration",
                table: "MotherboardM2Slots",
                columns: new[] { "MotherboardId", "Key", "PcieGeneration" },
                unique: true);
        }
    }
}
