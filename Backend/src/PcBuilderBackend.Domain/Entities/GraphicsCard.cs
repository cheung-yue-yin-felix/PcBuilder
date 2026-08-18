using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class GraphicsCard : ProductEntity
{
    public Guid GpuId { get; set; }
    public int VideoMemoryGb { get; set; }
    public int PcieSlotsUsed { get; set; }
    public PcieGeneration PcieGeneration { get; set; }
    public bool IsLowProfile { get; set; }
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public decimal PowerConsumptionWatts { get; set; }
    public Gpu Gpu { get; set; } = null!;
    public PsuCableType PowerConnectorType { get; set; }
    public int PowerConnectorCount { get; set; }

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
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        int powerConsumptionWatts,
        PsuCableType powerConnectorType,
        int powerConnectorCount)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(gpuId, videoMemoryGb, pcieSlotsUsed, pcieGeneration, lengthMm, widthMm, heightMm,
            powerConsumptionWatts, powerConnectorType, powerConnectorCount);
    }

    public void UpdateSpecs(
        Guid gpuId,
        int videoMemoryGb,
        int pcieSlotsUsed,
        PcieGeneration pcieGeneration,
        decimal lengthMm,
        decimal widthMm,
        decimal heightMm,
        int powerConsumptionWatts,
        PsuCableType powerConnectorType,
        int powerConnectorCount)
    {
        SetSpecs(gpuId, videoMemoryGb, pcieSlotsUsed, pcieGeneration, lengthMm, widthMm, heightMm,
            powerConsumptionWatts, powerConnectorType, powerConnectorCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        Guid gpuId,
        int videoMemoryGb,
        int pcieSlotsUsed,
        PcieGeneration pcieGeneration,
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
        PowerConsumptionWatts = powerConsumptionWatts;
        PowerConnectorType = powerConnectorType;
        PowerConnectorCount = powerConnectorCount;
    }
}
