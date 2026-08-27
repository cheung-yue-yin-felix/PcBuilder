namespace PcBuilderBackend.Application.Catalog.Psus.Dto;

public record PsuDto : PsuListItemDto
{
    public List<PsuCableDto> Cables { get; init; } = [];
}
