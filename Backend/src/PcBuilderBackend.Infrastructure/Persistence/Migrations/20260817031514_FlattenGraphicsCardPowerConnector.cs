using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FlattenGraphicsCardPowerConnector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PowerConnectorCount",
                table: "GraphicsCards",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<PsuCableType>(
                name: "PowerConnectorType",
                table: "GraphicsCards",
                type: "psu_cable_type",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE "GraphicsCards" AS g
                SET "PowerConnectorType" = c."PsuCableType",
                    "PowerConnectorCount" = c."ConnectorCount"
                FROM (
                    SELECT DISTINCT ON ("GraphicsCardId")
                        "GraphicsCardId",
                        "PsuCableType",
                        "ConnectorCount"
                    FROM "GraphicsCardPowerConnectors"
                    ORDER BY "GraphicsCardId", "IsActive" DESC, "CreatedAtUtc" DESC
                ) AS c
                WHERE g."Id" = c."GraphicsCardId";
                """);

            migrationBuilder.AlterColumn<int>(
                name: "PowerConnectorCount",
                table: "GraphicsCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<PsuCableType>(
                name: "PowerConnectorType",
                table: "GraphicsCards",
                type: "psu_cable_type",
                nullable: false,
                oldClrType: typeof(PsuCableType),
                oldType: "psu_cable_type",
                oldNullable: true);

            migrationBuilder.DropTable(
                name: "GraphicsCardPowerConnectors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GraphicsCardPowerConnectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false),
                    ConnectorCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GraphicsCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PsuCableType = table.Column<PsuCableType>(type: "psu_cable_type", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphicsCardPowerConnectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GraphicsCardPowerConnectors_GraphicsCards_GraphicsCardId",
                        column: x => x.GraphicsCardId,
                        principalTable: "GraphicsCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId_PsuCableType",
                table: "GraphicsCardPowerConnectors",
                columns: new[] { "GraphicsCardId", "PsuCableType" },
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO "GraphicsCardPowerConnectors" (
                    "Id",
                    "GraphicsCardId",
                    "PsuCableType",
                    "ConnectorCount",
                    "CreatedAtUtc",
                    "UpdatedAtUtc",
                    "IsActive",
                    "ConcurrencyToken")
                SELECT
                    gen_random_uuid(),
                    "Id",
                    "PowerConnectorType",
                    "PowerConnectorCount",
                    NOW() AT TIME ZONE 'utc',
                    NULL,
                    TRUE,
                    gen_random_uuid()
                FROM "GraphicsCards";
                """);

            migrationBuilder.DropColumn(
                name: "PowerConnectorCount",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "PowerConnectorType",
                table: "GraphicsCards");
        }
    }
}
