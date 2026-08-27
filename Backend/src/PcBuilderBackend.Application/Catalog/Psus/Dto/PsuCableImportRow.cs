using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Dto;

public class PsuCableImportRow
{
    public int ParentRowNumber { get; init; }
    public PsuCableType Type { get; init; }
    public int CablesCount { get; init; }
    public int ConnectorsCount { get; init; }
}
