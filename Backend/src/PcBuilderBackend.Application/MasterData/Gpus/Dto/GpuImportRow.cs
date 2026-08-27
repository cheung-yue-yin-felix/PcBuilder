namespace PcBuilderBackend.Application.MasterData.Gpus.Dto;

public class GpuImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public Guid SeriesId { get; init; }   
}