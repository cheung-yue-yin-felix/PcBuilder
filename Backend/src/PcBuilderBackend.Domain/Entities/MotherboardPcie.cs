using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardPcie : BaseEntity
{
    public Guid MotherboardId { get; set; }
    public PcieSlotType SlotType { get; set; }
    public PcieSlotLane SlotLanes { get; set; }
    public PcieGeneration Generation { get; set; }
    public int SlotCount { get; set; }

    protected MotherboardPcie() {}

    public MotherboardPcie(Guid motherboardId, PcieSlotType slotType, PcieSlotLane slotLanes, PcieGeneration generation, int slotCount)
    {
        SetSpecs(motherboardId, slotType, slotLanes, generation, slotCount);
    }

    public void UpdateSpecs(PcieSlotType slotType, PcieSlotLane slotLanes, PcieGeneration generation, int slotCount)
    {
        SetSpecs(MotherboardId, slotType, slotLanes, generation, slotCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid motherboardId, PcieSlotType slotType, PcieSlotLane slotLanes, PcieGeneration generation, int slotCount)
    {
        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID cannot be empty.", nameof(motherboardId));

        if (!Enum.IsDefined(slotType))
            throw new ArgumentException("PCIe slot type is invalid.", nameof(slotType));

        if (!Enum.IsDefined(slotLanes))
            throw new ArgumentException("PCIe slot lane is invalid.", nameof(slotLanes));

        if (!Enum.IsDefined(generation))
            throw new ArgumentException("PCIe generation is invalid.", nameof(generation));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(slotCount);

        MotherboardId = motherboardId;
        SlotType = slotType;
        SlotLanes = slotLanes;
        Generation = generation;
        SlotCount = slotCount;
    }
}
