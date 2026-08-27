using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public class CpuRamCompactImportRow
{
    public int ParentRowNumber { get; init; }
    public DdrGeneration DdrGeneration { get; init; }
    public int RamModuleCount { get; init; }
    public RamRank RamRank { get; init; }
    public int MaxSpeedMts { get; init; }
}