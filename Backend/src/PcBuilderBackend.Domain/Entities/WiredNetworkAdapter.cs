using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WiredNetworkAdapter : ProductEntity
{
    public WiredHostInterface HostInterface { get; private set; }
    public int MaxSpeedMbps { get; private set; }
    public UsbVersion? UsbVersion { get; private set; }
    public UsbType? UsbType { get; private set; }
    public PcieSlotType? PcieSlotType { get; private set; }
    
    protected WiredNetworkAdapter() { }

    public WiredNetworkAdapter(
        string name,
        Guid manufacturerId,
        WiredHostInterface hostInterface,
        int maxSpeedMbps,
        UsbVersion? usbVersion = null,
        UsbType? usbType = null,
        PcieSlotType? pcieSlotType = null)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(hostInterface, maxSpeedMbps, usbVersion, usbType, pcieSlotType);
    }

    public void UpdateSpecs(
        WiredHostInterface hostInterface,
        int maxSpeedMbps,
        UsbVersion? usbVersion = null,
        UsbType? usbType = null,
        PcieSlotType? pcieSlotType = null)
    {
        SetSpecs(hostInterface, maxSpeedMbps, usbVersion, usbType, pcieSlotType);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        WiredHostInterface hostInterface,
        int maxSpeedMbps,
        UsbVersion? usbVersion,
        UsbType? usbType,
        PcieSlotType? pcieSlotType)
    {
        if (!Enum.IsDefined(hostInterface))
            throw new ArgumentOutOfRangeException(nameof(hostInterface));
        
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps);

        switch (hostInterface)
        {
            case WiredHostInterface.Usb:
                if (usbVersion is null || !Enum.IsDefined(usbVersion.Value))
                    throw new ArgumentException("USB version is required for USB wired adapters.");
                if (usbType is null || !Enum.IsDefined(usbType.Value))
                    throw new ArgumentException("USB type is required for USB wired adapters.");
                if (pcieSlotType.HasValue)
                    throw new ArgumentException("PCIe slot type is not valid for USB wired adapters.");
                break;
            case WiredHostInterface.Pcie:
                if (pcieSlotType is null || !Enum.IsDefined(pcieSlotType.Value))
                    throw new ArgumentException("PCIe slot type is required for PCIe wired adapters.");
                if (usbVersion.HasValue || usbType.HasValue)
                    throw new ArgumentException("USB fields are not valid for PCIe wired adapters.");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hostInterface));
        }
        
        HostInterface = hostInterface;
        MaxSpeedMbps = maxSpeedMbps;
        UsbVersion = usbVersion;
        UsbType = usbType;
        PcieSlotType = pcieSlotType;
    }
}
