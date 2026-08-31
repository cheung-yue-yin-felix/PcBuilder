using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPcBuilds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'pc_build_part_type') THEN
                        CREATE TYPE pc_build_part_type AS ENUM (
                            'storage_drive',
                            'chassis_fan',
                            'wired_network_adapter',
                            'wireless_network_adapter');
                    END IF;
                END $$
                """);

            migrationBuilder.CreateTable(
                name: "PcBuilds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ChassisId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuCoolerId = table.Column<Guid>(type: "uuid", nullable: true),
                    RamKitId = table.Column<Guid>(type: "uuid", nullable: false),
                    GraphicsCardId = table.Column<Guid>(type: "uuid", nullable: true),
                    PsuId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcBuilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PcBuilds_Chassis_ChassisId",
                        column: x => x.ChassisId,
                        principalTable: "Chassis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_CpuCoolers_CpuCoolerId",
                        column: x => x.CpuCoolerId,
                        principalTable: "CpuCoolers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_GraphicsCards_GraphicsCardId",
                        column: x => x.GraphicsCardId,
                        principalTable: "GraphicsCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_Psus_PsuId",
                        column: x => x.PsuId,
                        principalTable: "Psus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PcBuilds_Rams_RamKitId",
                        column: x => x.RamKitId,
                        principalTable: "Rams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PcBuildParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PcBuildId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<PcBuildPartType>(type: "pc_build_part_type", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcBuildParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PcBuildParts_PcBuilds_PcBuildId",
                        column: x => x.PcBuildId,
                        principalTable: "PcBuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PcBuildUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PcBuildId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcBuildUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PcBuildUsers_PcBuilds_PcBuildId",
                        column: x => x.PcBuildId,
                        principalTable: "PcBuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PcBuildParts_PcBuildId_Type_PartId",
                table: "PcBuildParts",
                columns: new[] { "PcBuildId", "Type", "PartId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_ChassisId",
                table: "PcBuilds",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_CpuCoolerId",
                table: "PcBuilds",
                column: "CpuCoolerId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_CpuId",
                table: "PcBuilds",
                column: "CpuId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_GraphicsCardId",
                table: "PcBuilds",
                column: "GraphicsCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_MotherboardId",
                table: "PcBuilds",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_PsuId",
                table: "PcBuilds",
                column: "PsuId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuilds_RamKitId",
                table: "PcBuilds",
                column: "RamKitId");

            migrationBuilder.CreateIndex(
                name: "IX_PcBuildUsers_PcBuildId",
                table: "PcBuildUsers",
                column: "PcBuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PcBuildUsers_UserId",
                table: "PcBuildUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PcBuildParts");

            migrationBuilder.DropTable(
                name: "PcBuildUsers");

            migrationBuilder.DropTable(
                name: "PcBuilds");

            migrationBuilder.Sql("""
                DROP TYPE IF EXISTS pc_build_part_type;
                """);
        }
    }
}
