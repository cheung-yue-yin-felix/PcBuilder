using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.CpuCoolers;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.UpdateCpuCooler;

public record UpdateCpuCoolerCommand : IRequest<CpuCoolerDto?>, ICpuCoolerFields
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public int MaxTdp { get; init; }
    public CpuCoolerType Type { get; init; }
    public decimal? CoolerHeightMm { get; init; }
    public decimal? MaxRamHeightMm { get; init; }
    public RadiatorLength? RadiatorLength { get; init; }
}
