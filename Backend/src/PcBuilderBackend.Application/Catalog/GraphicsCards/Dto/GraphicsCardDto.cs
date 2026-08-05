namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;

public record GraphicsCardDto : GraphicsCardListItemDto
{
    public List<GraphicsCardPowerConnectorDto> PowerConnectors { get; init; } = [];
}
