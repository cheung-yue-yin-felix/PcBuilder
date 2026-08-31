namespace PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

public class GpuSeriesImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
}
