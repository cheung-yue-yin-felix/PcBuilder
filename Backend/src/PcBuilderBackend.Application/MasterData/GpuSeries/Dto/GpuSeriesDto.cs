namespace PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

public record GpuSeriesDto
{
    public Guid Id { get; set; }
    public Guid ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}