using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisDriveBayImportRow
{
    public int ParentRowNumber { get; init; }
    public DriveBayFormFactor FormFactor { get; init; }
    public int SlotCount { get; init; }
}
