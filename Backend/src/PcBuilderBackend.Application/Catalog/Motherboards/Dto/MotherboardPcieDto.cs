using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Dto;

public record MotherboardPcieDto
{
    public PcieSlotType SlotType { get; init; }
    public PcieSlotLane SlotLanes { get; init; }
    public PcieGeneration Generation { get; init; }
    public int SlotCount { get; init; }
}
