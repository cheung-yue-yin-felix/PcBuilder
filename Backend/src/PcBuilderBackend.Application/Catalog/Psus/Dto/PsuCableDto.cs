using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Dto;

public record PsuCableDto
{
    public PsuCableType Type { get; init; }
    public int CablesCount { get; init; }
    public int ConnectorsCount { get; init; }
}
