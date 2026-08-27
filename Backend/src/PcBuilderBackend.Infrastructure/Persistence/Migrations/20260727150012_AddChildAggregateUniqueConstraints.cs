using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PcBuilderBackend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChildAggregateUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuRamMaxSpeed_Cpus_CpuId",
                table: "CpuRamMaxSpeed");

            migrationBuilder.DropIndex(
                name: "IX_PsuCables_PsuId",
                table: "PsuCables");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardPcieSlots_MotherboardId",
                table: "MotherboardPcieSlots");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId",
                table: "MotherboardM2Slots");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2FormFactors_MotherboardM2Id",
                table: "MotherboardM2FormFactors");

            migrationBuilder.DropIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId",
                table: "GraphicsCardPowerConnectors");

            migrationBuilder.DropIndex(
                name: "IX_CpuCoolerSockets_CpuCoolerId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropIndex(
                name: "IX_ChassisRadiators_ChassisId",
                table: "ChassisRadiators");

            migrationBuilder.DropIndex(
                name: "IX_ChassisPsuFormFactor_ChassisId",
                table: "ChassisPsuFormFactor");

            migrationBuilder.DropIndex(
                name: "IX_ChassisPcieSlots_ChassisId",
                table: "ChassisPcieSlots");

            migrationBuilder.DropIndex(
                name: "IX_ChassisMbFormFactor_ChassisId",
                table: "ChassisMbFormFactor");

            migrationBuilder.DropIndex(
                name: "IX_ChassisFanMounts_ChassisId",
                table: "ChassisFanMounts");

            migrationBuilder.DropIndex(
                name: "IX_ChassisFanMountOptions_ChassisFanMountId",
                table: "ChassisFanMountOptions");

            migrationBuilder.DropIndex(
                name: "IX_ChassisDriveBays_ChassisId",
                table: "ChassisDriveBays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CpuRamMaxSpeed",
                table: "CpuRamMaxSpeed");

            migrationBuilder.DropIndex(
                name: "IX_CpuRamMaxSpeed_CpuId",
                table: "CpuRamMaxSpeed");

            migrationBuilder.RenameTable(
                name: "CpuRamMaxSpeed",
                newName: "CpuRamMaxSpeeds");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CpuRamMaxSpeeds",
                table: "CpuRamMaxSpeeds",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PsuCables_PsuId_Type",
                table: "PsuCables",
                columns: new[] { "PsuId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPcieSlots_MotherboardId_SlotType_SlotLanes_Gener~",
                table: "MotherboardPcieSlots",
                columns: new[] { "MotherboardId", "SlotType", "SlotLanes", "Generation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_PcieGeneration",
                table: "MotherboardM2Slots",
                columns: new[] { "MotherboardId", "PcieGeneration" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2FormFactors_MotherboardM2Id_FormFactor",
                table: "MotherboardM2FormFactors",
                columns: new[] { "MotherboardM2Id", "FormFactor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId_PsuCableType",
                table: "GraphicsCardPowerConnectors",
                columns: new[] { "GraphicsCardId", "PsuCableType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoolerSockets_CpuCoolerId_SocketId",
                table: "CpuCoolerSockets",
                columns: new[] { "CpuCoolerId", "SocketId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisRadiators_ChassisId_Length_MountLocation",
                table: "ChassisRadiators",
                columns: new[] { "ChassisId", "Length", "MountLocation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPsuFormFactor_ChassisId_PsuFormFactor",
                table: "ChassisPsuFormFactor",
                columns: new[] { "ChassisId", "PsuFormFactor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPcieSlots_ChassisId_LowProfileSlots_Orientation",
                table: "ChassisPcieSlots",
                columns: new[] { "ChassisId", "LowProfileSlots", "Orientation" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisMbFormFactor_ChassisId_MbFormFactor",
                table: "ChassisMbFormFactor",
                columns: new[] { "ChassisId", "MbFormFactor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMounts_ChassisId_Location",
                table: "ChassisFanMounts",
                columns: new[] { "ChassisId", "Location" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMountOptions_ChassisFanMountId_Diameter",
                table: "ChassisFanMountOptions",
                columns: new[] { "ChassisFanMountId", "Diameter" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChassisDriveBays_ChassisId_DriveBayFormFactor",
                table: "ChassisDriveBays",
                columns: new[] { "ChassisId", "DriveBayFormFactor" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CpuRamMaxSpeeds_CpuId_RamModuleCount_RamRank",
                table: "CpuRamMaxSpeeds",
                columns: new[] { "CpuId", "RamModuleCount", "RamRank" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CpuRamMaxSpeeds_Cpus_CpuId",
                table: "CpuRamMaxSpeeds",
                column: "CpuId",
                principalTable: "Cpus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CpuRamMaxSpeeds_Cpus_CpuId",
                table: "CpuRamMaxSpeeds");

            migrationBuilder.DropIndex(
                name: "IX_PsuCables_PsuId_Type",
                table: "PsuCables");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardPcieSlots_MotherboardId_SlotType_SlotLanes_Gener~",
                table: "MotherboardPcieSlots");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2Slots_MotherboardId_PcieGeneration",
                table: "MotherboardM2Slots");

            migrationBuilder.DropIndex(
                name: "IX_MotherboardM2FormFactors_MotherboardM2Id_FormFactor",
                table: "MotherboardM2FormFactors");

            migrationBuilder.DropIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId_PsuCableType",
                table: "GraphicsCardPowerConnectors");

            migrationBuilder.DropIndex(
                name: "IX_CpuCoolerSockets_CpuCoolerId_SocketId",
                table: "CpuCoolerSockets");

            migrationBuilder.DropIndex(
                name: "IX_ChassisRadiators_ChassisId_Length_MountLocation",
                table: "ChassisRadiators");

            migrationBuilder.DropIndex(
                name: "IX_ChassisPsuFormFactor_ChassisId_PsuFormFactor",
                table: "ChassisPsuFormFactor");

            migrationBuilder.DropIndex(
                name: "IX_ChassisPcieSlots_ChassisId_LowProfileSlots_Orientation",
                table: "ChassisPcieSlots");

            migrationBuilder.DropIndex(
                name: "IX_ChassisMbFormFactor_ChassisId_MbFormFactor",
                table: "ChassisMbFormFactor");

            migrationBuilder.DropIndex(
                name: "IX_ChassisFanMounts_ChassisId_Location",
                table: "ChassisFanMounts");

            migrationBuilder.DropIndex(
                name: "IX_ChassisFanMountOptions_ChassisFanMountId_Diameter",
                table: "ChassisFanMountOptions");

            migrationBuilder.DropIndex(
                name: "IX_ChassisDriveBays_ChassisId_DriveBayFormFactor",
                table: "ChassisDriveBays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CpuRamMaxSpeeds",
                table: "CpuRamMaxSpeeds");

            migrationBuilder.DropIndex(
                name: "IX_CpuRamMaxSpeeds_CpuId_RamModuleCount_RamRank",
                table: "CpuRamMaxSpeeds");

            migrationBuilder.RenameTable(
                name: "CpuRamMaxSpeeds",
                newName: "CpuRamMaxSpeed");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CpuRamMaxSpeed",
                table: "CpuRamMaxSpeed",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PsuCables_PsuId",
                table: "PsuCables",
                column: "PsuId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardPcieSlots_MotherboardId",
                table: "MotherboardPcieSlots",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2Slots_MotherboardId",
                table: "MotherboardM2Slots",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherboardM2FormFactors_MotherboardM2Id",
                table: "MotherboardM2FormFactors",
                column: "MotherboardM2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GraphicsCardPowerConnectors_GraphicsCardId",
                table: "GraphicsCardPowerConnectors",
                column: "GraphicsCardId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoolerSockets_CpuCoolerId",
                table: "CpuCoolerSockets",
                column: "CpuCoolerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisRadiators_ChassisId",
                table: "ChassisRadiators",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPsuFormFactor_ChassisId",
                table: "ChassisPsuFormFactor",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisPcieSlots_ChassisId",
                table: "ChassisPcieSlots",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisMbFormFactor_ChassisId",
                table: "ChassisMbFormFactor",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMounts_ChassisId",
                table: "ChassisFanMounts",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisFanMountOptions_ChassisFanMountId",
                table: "ChassisFanMountOptions",
                column: "ChassisFanMountId");

            migrationBuilder.CreateIndex(
                name: "IX_ChassisDriveBays_ChassisId",
                table: "ChassisDriveBays",
                column: "ChassisId");

            migrationBuilder.CreateIndex(
                name: "IX_CpuRamMaxSpeed_CpuId",
                table: "CpuRamMaxSpeed",
                column: "CpuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CpuRamMaxSpeed_Cpus_CpuId",
                table: "CpuRamMaxSpeed",
                column: "CpuId",
                principalTable: "Cpus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
