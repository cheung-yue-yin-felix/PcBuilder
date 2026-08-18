using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardM2 : BaseEntity
{
    public Guid MotherboardId { get; set; }
    public M2Key Key { get; set; }
    public PcieGeneration PcieGeneration { get; set; }
    public int SlotCount { get; set; }
    public bool SupportsSata { get; set; }
    public ICollection<MotherboardM2FormFactor> FormFactors { get; set; } = new List<MotherboardM2FormFactor>();

    protected MotherboardM2() {}

    public MotherboardM2(Guid motherboardId, M2Key key, PcieGeneration pcieGeneration, int slotCount, bool supportsSata)
    {
        SetSpecs(motherboardId, key, pcieGeneration, slotCount, supportsSata);
    }

    public void UpdateSpecs(M2Key key, PcieGeneration pcieGeneration, int slotCount, bool supportsSata)
    {
        SetSpecs(MotherboardId, key, pcieGeneration, slotCount, supportsSata);
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

    private void SetSpecs(Guid motherboardId, M2Key key, PcieGeneration pcieGeneration, int slotCount, bool supportsSata)
    {
        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID cannot be empty.", nameof(motherboardId));

        if (!key.IsSlotKey())
            throw new ArgumentException("M.2 slot key must be M, B, or E.", nameof(key));

        if (!Enum.IsDefined(pcieGeneration))
            throw new ArgumentException("PCIe generation is invalid.", nameof(pcieGeneration));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(slotCount);

        if (key == M2Key.E && supportsSata)
            throw new ArgumentException("E-key slots cannot support SATA.", nameof(supportsSata));

        MotherboardId = motherboardId;
        Key = key;
        PcieGeneration = pcieGeneration;
        SlotCount = slotCount;
        SupportsSata = supportsSata;
    }
}
