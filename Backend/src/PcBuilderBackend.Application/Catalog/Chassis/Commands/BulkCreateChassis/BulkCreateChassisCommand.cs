using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.BulkCreateChassis;

public record BulkCreateChassisCommand(List<CreateChassisItem> Items) : IRequest<List<ChassisDto>>;

public record CreateChassisItem
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public decimal LengthMm { get; init; }
    public decimal WidthMm { get; init; }
    public decimal HeightMm { get; init; }
    public decimal MotherboardMaxWidthMm { get; init; }
    public decimal MotherboardMaxHeightMm { get; init; }
    public decimal MaxCpuCoolerHeightMm { get; init; }
    public decimal MaxGraphicsCardLengthMm { get; init; }
    public decimal MaxPsuLengthMm { get; init; }
    public List<ChassisFanMountDto> FanMounts { get; init; } = [];
    public List<ChassisDriveBayDto> DriveBays { get; init; } = [];
    public List<ChassisPcieSlotDto> PcieSlots { get; init; } = [];
    public List<ChassisRadiatorDto> Radiators { get; init; } = [];
    public List<PsuFormFactor> PsuFormFactors { get; init; } = [];
    public List<MbFormFactor> MbFormFactors { get; init; } = [];
}
