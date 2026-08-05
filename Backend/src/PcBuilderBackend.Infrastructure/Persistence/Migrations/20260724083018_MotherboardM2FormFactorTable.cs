using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using PcBuilderBackend.Domain.Enums;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MotherboardM2FormFactorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormFactors",
                table: "MotherboardM2Slots");

            migrationBuilder.CreateTable(
                name: "MotherboardM2FormFactors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherboardM2Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormFactor = table.Column<M2FormFactor>(type: "m2_form_factor", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConcurrencyToken = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherboardM2FormFactors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherboardM2FormFactors_MotherboardM2Slots_MotherboardM2Id",
                        column: x => x.MotherboardM2Id,
                        principalTable: "MotherboardM2Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2FormFactors_MotherboardM2Id",
                table: "MotherboardM2FormFactors",
                column: "MotherboardM2Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherboardM2FormFactors");

            migrationBuilder.AddColumn<List<M2FormFactor>>(
                name: "FormFactors",
                table: "MotherboardM2Slots",
                type: "m2_form_factor[]",
                nullable: false);
        }
    }
}
