using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCpuSupportChipsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets");

            migrationBuilder.CreateTable(
                name: "CpuSupportChipsets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CpuId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChipsetId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequiresBiosUpdate = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuSupportChipsets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CpuSupportChipsets_Chipsets_ChipsetId",
                        column: x => x.ChipsetId,
                        principalTable: "Chipsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CpuSupportChipsets_Cpus_CpuId",
                        column: x => x.CpuId,
                        principalTable: "Cpus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuSupportChipsets_ChipsetId",
                table: "CpuSupportChipsets",
                column: "ChipsetId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuSupportChipsets_CpuId_ChipsetId",
                table: "CpuSupportChipsets",
                columns: new[] { "CpuId", "ChipsetId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets");

            migrationBuilder.DropTable(
                name: "CpuSupportChipsets");

            migrationBuilder.AddForeignKey(
                name: "FK_Chipsets_Sockets_SocketId",
                table: "Chipsets",
                column: "SocketId",
                principalTable: "Sockets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
