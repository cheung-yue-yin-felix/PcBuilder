using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMotherboardM2Key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'm2_key') THEN
                        CREATE TYPE m2_key AS ENUM ('m', 'b', 'e', 'bm');
                    END IF;
                END $$;
                """);

            migrationBuilder.AddColumn<M2Key>(
                name: "Key",
                table: "MotherboardM2Slots",
                type: "m2_key",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsSata",
                table: "MotherboardM2Slots",
                type: "boolean",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "MotherboardM2Slots"
                SET "Key" = 'm'::m2_key,
                    "SupportsSata" = false
                WHERE "Key" IS NULL;
                """);

            migrationBuilder.AlterColumn<M2Key>(
                name: "Key",
                table: "MotherboardM2Slots",
                type: "m2_key",
                nullable: false,
                oldClrType: typeof(M2Key),
                oldType: "m2_key",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "SupportsSata",
                table: "MotherboardM2Slots",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_PcieGeneration",
                table: "MotherboardM2Slots");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_Key_PcieGeneration",
                table: "MotherboardM2Slots",
                columns: new[] { "MotherboardId", "Key", "PcieGeneration" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_Key_PcieGeneration",
                table: "MotherboardM2Slots");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "MotherboardM2Slots");

            migrationBuilder.DropColumn(
                name: "SupportsSata",
                table: "MotherboardM2Slots");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_PcieGeneration",
                table: "MotherboardM2Slots",
                columns: new[] { "MotherboardId", "PcieGeneration" },
                unique: true);

            migrationBuilder.Sql("DROP TYPE IF EXISTS m2_key;");
        }
    }
}
