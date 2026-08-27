namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

public record CpuCoolerDto : CpuCoolerListItemDto
{
    public List<CpuCoolerSocketDto> Sockets { get; init; } = [];
}
