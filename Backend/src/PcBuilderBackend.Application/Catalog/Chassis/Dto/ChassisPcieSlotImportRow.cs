using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Chassis.Dto;

public class ChassisPcieSlotImportRow
{
    public int ParentRowNumber { get; init; }
    public bool LowProfileSlots { get; init; }
    public int SlotCount { get; init; }
    public PcieOrientation Orientation { get; init; }
}
