namespace PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

public class CpuSeriesImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SocketId { get; init; }
}
