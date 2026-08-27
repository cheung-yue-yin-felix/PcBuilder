using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.CreateCpuCooler;

public record CreateCpuCoolerCommand : IRequest<CpuCoolerDto>
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public int MaxTdp { get; init; }
    public CpuCoolerType Type { get; init; }
    public decimal? CoolerHeightMm { get; init; }
    public decimal? MaxRamHeightMm { get; init; }
    public RadiatorLength? RadiatorLength { get; init; }
    public List<CpuCoolerSocketDto> Sockets { get; init; } = [];
}
