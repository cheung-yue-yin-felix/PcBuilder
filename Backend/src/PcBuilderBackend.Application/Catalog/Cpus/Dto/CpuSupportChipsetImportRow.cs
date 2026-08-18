namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public class CpuSupportChipsetImportRow
{
    public int ParentRowNumber { get; init; }
    public Guid ChipsetId { get; init; }
    public bool RequiresBiosUpdate { get; init; }
}
