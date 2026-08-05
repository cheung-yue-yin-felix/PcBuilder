using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;

public record CreateMemoryCommand : IRequest<RamDto>
{
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public string Color { get; init; } = string.Empty;
    public DdrGeneration DdrGeneration { get; init; }
    public RamFormFactor RamFormFactor { get; init; }
    public RamRank RamRank { get; init; }
    public int MemorySizePerStickGb { get; init; }
    public int TotalMemorySizeGb { get; init; }
    public int ModulesCount { get; init; }
    public int MaxMemorySpeedMts { get; init; }
    public decimal HeightMm { get; init; }
}
