using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardM2 : BaseEntity
{
    public Guid MotherboardId { get; set; }
    public PcieGeneration PcieGeneration { get; set; }
    public int SlotCount { get; set; }
    public ICollection<MotherboardM2FormFactor> FormFactors { get; set; } = new List<MotherboardM2FormFactor>();

    protected MotherboardM2() {}

    public MotherboardM2(Guid motherboardId, PcieGeneration pcieGeneration, int slotCount)
    {
        SetSpecs(motherboardId, pcieGeneration, slotCount);
    }

    public void UpdateSpecs(PcieGeneration pcieGeneration, int slotCount)
    {
        SetSpecs(MotherboardId, pcieGeneration, slotCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddFormFactor(MotherboardM2FormFactor formFactor)
    {
        if (FormFactors.Any(x => x.FormFactor == formFactor.FormFactor))
            throw new InvalidOperationException("M.2 form factor already exists.");

        FormFactors.Add(formFactor);
    }

    public void RemoveFormFactor(MotherboardM2FormFactor formFactor)
    {
        if (!FormFactors.Any(x => x.FormFactor == formFactor.FormFactor))
            throw new InvalidOperationException("M.2 form factor does not exist.");

        FormFactors.Remove(formFactor);
    }

    private void SetSpecs(Guid motherboardId, PcieGeneration pcieGeneration, int slotCount)
    {
        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID cannot be empty.", nameof(motherboardId));

        if (!Enum.IsDefined(pcieGeneration))
            throw new ArgumentException("PCIe generation is invalid.", nameof(pcieGeneration));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(slotCount);

        MotherboardId = motherboardId;
        PcieGeneration = pcieGeneration;
        SlotCount = slotCount;
    }
}
