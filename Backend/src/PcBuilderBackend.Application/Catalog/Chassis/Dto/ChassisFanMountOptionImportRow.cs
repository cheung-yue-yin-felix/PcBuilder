using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisFanMountOptionImportRow
{
    public int ParentRowNumber { get; init; }
    public FanDiameterMm Diameter { get; init; }
    public int SlotCount { get; init; }
}
