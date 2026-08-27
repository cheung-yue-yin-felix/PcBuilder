using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Dto;

public class ChassisFanImportRow
{
    public int RowNumber { get; init; }
    public string Name { get; init; } = string.Empty;
    public Guid ManufacturerId { get; init; }
    public FanDiameterMm DiameterMm { get; init; }
    public int FansCountPerPack { get; init; }
}
