using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Application.Catalog.Memories;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;

public record UpdateMemoryCommand : IRequest<RamDto?>, IMemoryFields
{
    public Guid Id { get; init; }
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
