using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Series",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "Series",
                table: "Cpus");

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Gpus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Gpus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Cpus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "CpuSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufacturerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SocketId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GpuSeries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GpuSeries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_SeriesId",
                table: "Gpus",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cpus_SeriesId",
                table: "Cpus",
                column: "SeriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cpus_CpuSeries_SeriesId",
                table: "Cpus",
                column: "SeriesId",
                principalTable: "CpuSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_GpuSeries_SeriesId",
                table: "Gpus",
                column: "SeriesId",
                principalTable: "GpuSeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cpus_CpuSeries_SeriesId",
                table: "Cpus");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_GpuSeries_SeriesId",
                table: "Gpus");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropTable(
                name: "CpuSeries");

            migrationBuilder.DropTable(
                name: "GpuSeries");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_SeriesId",
                table: "Gpus");

            migrationBuilder.DropIndex(
                name: "IX_Cpus_SeriesId",
                table: "Cpus");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Cpus");

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Gpus",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Series",
                table: "Cpus",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
