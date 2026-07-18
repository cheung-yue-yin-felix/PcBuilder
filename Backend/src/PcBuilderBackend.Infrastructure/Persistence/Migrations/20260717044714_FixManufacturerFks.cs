using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixManufacturerFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chassis_Manufacturers_ManufacturerId1",
                table: "Chassis");

            migrationBuilder.DropForeignKey(
                name: "FK_Chipsets_Manufacturers_ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropForeignKey(
                name: "FK_GraphicsCards_Manufacturers_ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Motherboards_Manufacturers_ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Psus_Manufacturers_ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropForeignKey(
                name: "FK_Rams_Manufacturers_ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropIndex(
                name: "IX_Rams_ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropIndex(
                name: "IX_Psus_ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropIndex(
                name: "IX_Motherboards_ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropIndex(
                name: "IX_GraphicsCards_ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropIndex(
                name: "IX_Chipsets_ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropIndex(
                name: "IX_Chassis_ManufacturerId1",
                table: "Chassis");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Rams");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Psus");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Motherboards");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "GraphicsCards");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Gpus");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Chipsets");

            migrationBuilder.DropColumn(
                name: "ManufacturerId1",
                table: "Chassis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Rams",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Psus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Motherboards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "GraphicsCards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Gpus",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Chipsets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ManufacturerId1",
                table: "Chassis",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rams_ManufacturerId1",
                table: "Rams",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Psus_ManufacturerId1",
                table: "Psus",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Motherboards_ManufacturerId1",
                table: "Motherboards",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCards_ManufacturerId1",
                table: "GraphicsCards",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Gpus_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Chipsets_ManufacturerId1",
                table: "Chipsets",
                column: "ManufacturerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Chassis_ManufacturerId1",
                table: "Chassis",
                column: "ManufacturerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Chassis_Manufacturers_ManufacturerId1",
                table: "Chassis",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chipsets_Manufacturers_ManufacturerId1",
                table: "Chipsets",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gpus_Manufacturers_ManufacturerId1",
                table: "Gpus",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphicsCards_Manufacturers_ManufacturerId1",
                table: "GraphicsCards",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Motherboards_Manufacturers_ManufacturerId1",
                table: "Motherboards",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Psus_Manufacturers_ManufacturerId1",
                table: "Psus",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rams_Manufacturers_ManufacturerId1",
                table: "Rams",
                column: "ManufacturerId1",
                principalTable: "Manufacturers",
                principalColumn: "Id");
        }
    }
}
