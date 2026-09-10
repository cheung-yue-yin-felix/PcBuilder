using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Cpus;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;

public record CreateCpuCommand : IRequest<CpuDto>, ICpuFields
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
    public Guid SeriesId { get; init; }
    public int MaxMemoryGb { get; init; }
    public bool IntegratedGraphics { get; init; }
    public bool IncludedStockCooler { get; init; }
    public int ThermalDesignPower { get; init; }
    public int PowerConsumptionWatts { get; init; }
    public List<CpuRamCompatDto> RamCompats { get; init; } = [];
    public List<CpuSupportChipsetDto> SupportChipsets { get; init; } = [];
}
