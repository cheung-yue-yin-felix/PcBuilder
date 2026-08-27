using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardM2FormFactor : BaseEntity
{
    public Guid MotherboardM2Id { get; private set; }
    public M2FormFactor FormFactor { get; private set; }
    public MotherboardM2 MotherboardM2 { get; private set; } = null!;

    protected MotherboardM2FormFactor() {}

    public MotherboardM2FormFactor(Guid motherboardM2Id, M2FormFactor formFactor)
    {
        SetSpecs(motherboardM2Id, formFactor);
    }

    public void UpdateSpecs(Guid motherboardM2Id, M2FormFactor formFactor)
    {
        SetSpecs(motherboardM2Id, formFactor);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid motherboardM2Id, M2FormFactor formFactor)
    {
        if (motherboardM2Id == Guid.Empty)
            throw new ArgumentException("Motherboard M.2 ID cannot be empty.", nameof(motherboardM2Id));

        if (!Enum.IsDefined(formFactor))
            throw new ArgumentException("M.2 form factor is invalid.", nameof(formFactor));

        MotherboardM2Id = motherboardM2Id;
        FormFactor = formFactor;
    }
}
