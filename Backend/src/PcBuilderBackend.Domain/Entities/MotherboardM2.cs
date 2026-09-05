using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardM2 : BaseEntity
{
    public Guid MotherboardId { get; set; }
    public M2Key Key { get; set; }
    public PcieGeneration PcieGeneration { get; set; }
    public int SlotCount { get; set; }
    public bool SupportsSata { get; set; }

    private readonly List<MotherboardM2FormFactor> _formFactors = new();
    public IReadOnlyCollection<MotherboardM2FormFactor> FormFactors => _formFactors;

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
        if (_formFactors.Any(x => x.FormFactor == formFactor.FormFactor))
            throw new ArgumentException("M.2 form factor already exists.");

        _formFactors.Add(formFactor);
    }

    public void RemoveFormFactor(MotherboardM2FormFactor formFactor)
    {
        if (_formFactors.All(x => x.FormFactor != formFactor.FormFactor))
            throw new ArgumentException("M.2 form factor does not exist.");

        _formFactors.Remove(formFactor);
    }

    public MotherboardM2GroupKey GroupKey() =>
        MotherboardM2GroupKey.From(Key, PcieGeneration, SupportsSata, FormFactors.Select(x => x.FormFactor));

    public bool IsSameSlotGroup(MotherboardM2 other)
    {
        ArgumentNullException.ThrowIfNull(other);
        return GroupKey() == other.GroupKey();
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
