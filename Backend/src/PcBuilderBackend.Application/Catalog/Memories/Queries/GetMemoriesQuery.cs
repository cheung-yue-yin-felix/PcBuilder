using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public class GetMemoriesQuery : IRequest<List<RamDto>>
{
    public string? Name { get; init; } = string.Empty;
    public Guid? ManufacturerId { get; init; }
    public string? Color { get; set; } = string.Empty;
    public DdrGeneration? DdrGeneration { get; set; }
    public RamFormFactor? RamFormFactor { get; set; }
    public RamRank? RamRank { get; set; }
    public int? MemorySizePerStickGb { get; set; }
    public int? TotalMemorySizeGb { get; set; }
    public int? ModulesCount { get; set; }
    public int? MaxMemorySpeedMts { get; set; }
    public decimal? MinHeightMm { get; set; }
    public decimal? MaxHeightMm { get; set; }
}
