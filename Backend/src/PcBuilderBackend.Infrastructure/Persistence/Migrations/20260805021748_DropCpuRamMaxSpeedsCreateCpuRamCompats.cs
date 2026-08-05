using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropCpuRamMaxSpeedsCreateCpuRamCompats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuSeries_Sockets_SocketId",
                table: "CpuSeries");

            migrationBuilder.DropTable(
                name: "CpuRamMaxSpeeds");

            migrationBuilder.DropColumn(
                name: "DdrGeneration",
                table: "Cpus");

            migrationBuilder.CreateTable(
                name: "CpuRamCompats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    DdrGeneration = table.Column<DdrGeneration>(type: "ddr_generation", nullable: false),
                    RamModuleCount = table.Column<int>(type: "integer", nullable: false),
                    RamRank = table.Column<RamRank>(type: "ram_rank", nullable: false),
                    MaxSpeedMts = table.Column<int>(type: "integer", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuRamCompats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuRamCompats_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuRamCompats_CpuId_DdrGeneration_RamModuleCount_RamRank",
                table: "CpuRamCompats",
                columns: new[] { "CpuId", "DdrGeneration", "RamModuleCount", "RamRank" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuSeries_Sockets_SocketId",
                table: "CpuSeries",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuSeries_Sockets_SocketId",
                table: "CpuSeries");

            migrationBuilder.DropTable(
                name: "CpuRamCompats");

            migrationBuilder.AddColumn<DdrGeneration>(
                name: "DdrGeneration",
                table: "Cpus",
                type: "ddr_generation",
                nullable: false,
                defaultValue: (DdrGeneration)0);

            migrationBuilder.CreateTable(
                name: "CpuRamMaxSpeeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MaxSpeedMts = table.Column<int>(type: "integer", nullable: false),
                    RamModuleCount = table.Column<int>(type: "integer", nullable: false),
                    RamRank = table.Column<RamRank>(type: "ram_rank", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuRamMaxSpeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuRamMaxSpeeds_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuRamMaxSpeeds_CpuId_RamModuleCount_RamRank",
                table: "CpuRamMaxSpeeds",
                columns: new[] { "CpuId", "RamModuleCount", "RamRank" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuSeries_Sockets_SocketId",
                table: "CpuSeries",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
