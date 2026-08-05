using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

public record UpdateCpuCommand : IRequest<CpuDto?>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
    public Guid SeriesId { get; init; }
    public int MaxMemoryGb { get; init; }
    public bool IntegratedGraphics { get; init; }
    public bool IncludedStockCooler { get; init; }
    public int ThermalDesignPower { get; init; }
}
