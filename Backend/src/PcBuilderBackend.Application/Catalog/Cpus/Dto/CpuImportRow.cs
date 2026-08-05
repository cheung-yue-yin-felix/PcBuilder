namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public class CpuImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
    public Guid SeriesId { get; init; }
    public int MaxMemoryGb { get; init; }
    public bool IntegratedGraphics { get; init; }
    public bool IncludedStockCooler { get; init; }
    public int ThermalDesignPower { get; init; }
    public List<CpuRamCompactImportRow> RamCompats { get; init; } = [];
}
