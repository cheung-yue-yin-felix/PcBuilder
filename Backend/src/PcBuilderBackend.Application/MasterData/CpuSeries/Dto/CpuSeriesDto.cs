namespace PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

public record CpuSeriesDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ManufacturerId { get; set; }
    public Guid SocketId { get; set; }
}