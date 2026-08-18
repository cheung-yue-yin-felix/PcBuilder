namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

public record GraphicsCardDto : GraphicsCardListItemDto
{
    public Guid GpuManufacturerId { get; init; }
    public string GpuManufacturerName { get; init; } = string.Empty;
    public Guid GpuSeriesId { get; init; }
    public string GpuSeriesName { get; init; } = string.Empty;
}
