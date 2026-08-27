using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisFanMountImportRow
{
    public int RowNumber { get; init; }
    public int ParentRowNumber { get; init; }
    public FanMountLocation Location { get; init; }
    public bool SingleDiameterOnly { get; init; }
    public List<ChassisFanMountOptionImportRow> Options { get; init; } = [];
}
