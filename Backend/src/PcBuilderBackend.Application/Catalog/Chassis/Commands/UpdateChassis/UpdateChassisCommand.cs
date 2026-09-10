using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Catalog.Chassis;

namespace PcBuilderBackend.Application.Catalog.Chassis.Commands.UpdateChassis;

public record UpdateChassisCommand : IRequest<ChassisDto?>, IChassisFields
{
    public Guid Id { get; init; }
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
}
