using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class GraphicsCard : ProductEntity
{
    public Guid GpuId { get; private set; }
    public int VideoMemoryGb { get; private set; }
    public int PcieSlotsUsed { get; private set; }
    public PcieGeneration PcieGeneration { get; private set; }
    public bool IsLowProfile { get; private set; }
    public decimal LengthMm { get; private set; }
    public decimal WidthMm { get; private set; }
    public decimal HeightMm { get; private set; }
    public int PowerConsumptionWatts { get; private set; }
    public Gpu Gpu { get; private set; } = null!;
    public PsuCableType PowerConnectorType { get; private set; }
    public int PowerConnectorCount { get; private set; }

    protected GraphicsCard()
    {
    }

    public GraphicsCard(
        string name,
        Guid manufacturerId,
        Guid gpuId,
        int videoMemoryGb,
        int pcieSlotsUsed,
        PcieGeneration pcieGeneration,
        bool isLowProfile,
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        int powerConsumptionWatts,
        PsuCableType powerConnectorType,
        int powerConnectorCount)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(gpuId, videoMemoryGb, pcieSlotsUsed, pcieGeneration, isLowProfile, lengthMm, widthMm, heightMm,
            powerConsumptionWatts, powerConnectorType, powerConnectorCount);
    }

    public void UpdateSpecs(
        Guid gpuId,
        int videoMemoryGb,
        int pcieSlotsUsed,
        PcieGeneration pcieGeneration,
        bool isLowProfile,
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        int powerConsumptionWatts,
        PsuCableType powerConnectorType,
        int powerConnectorCount)
    {
        SetSpecs(gpuId, videoMemoryGb, pcieSlotsUsed, pcieGeneration, isLowProfile, lengthMm, widthMm, heightMm,
            powerConsumptionWatts, powerConnectorType, powerConnectorCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        Guid gpuId,
        int videoMemoryGb,
        int pcieSlotsUsed,
        PcieGeneration pcieGeneration,
        bool isLowProfile,
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        int powerConsumptionWatts,
        PsuCableType powerConnectorType,
        int powerConnectorCount)
    {
        if (gpuId == Guid.Empty)
            throw new ArgumentException("Gpu id can't be empty");

        if (videoMemoryGb <= 0)
            throw new ArgumentException("Video memory GB must be greater than zero");

        if (pcieSlotsUsed <= 0)
            throw new ArgumentException("PcieSlotsUsed must be greater than zero");

        if (!Enum.IsDefined(pcieGeneration))
            throw new ArgumentException("PcieGeneration must be a valid value");

        if (lengthMm <= 0)
            throw new ArgumentException("Length mm must be greater than zero");

        if (widthMm <= 0)
            throw new ArgumentException("Width mm must be greater than zero");

        if (heightMm <= 0)
            throw new ArgumentException("Height mm must be greater than zero");

        if (powerConsumptionWatts <= 0)
            throw new ArgumentException("PowerConsumptionWatts must be greater than zero");

        if (!Enum.IsDefined(powerConnectorType))
            throw new ArgumentException("PowerConnectorType must be a valid value");

        if (powerConnectorCount <= 0)
            throw new ArgumentException("PowerConnectorCount must be greater than zero");

        GpuId = gpuId;
        VideoMemoryGb = videoMemoryGb;
        PcieSlotsUsed = pcieSlotsUsed;
        PcieGeneration = pcieGeneration;
        LengthMm = lengthMm;
        WidthMm = widthMm;
        HeightMm = heightMm;
        IsLowProfile = isLowProfile;
        PowerConsumptionWatts = powerConsumptionWatts;
        PowerConnectorType = powerConnectorType;
        PowerConnectorCount = powerConnectorCount;
    }
}
