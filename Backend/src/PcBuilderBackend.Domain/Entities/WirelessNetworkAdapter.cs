using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class WirelessNetworkAdapter : ProductEntity
{
    public WifiStandard WifiStandard { get; set; }
    public BluetoothVersion? BluetoothVersion { get; set; }
    public WirelessHostInterface HostInterface { get; set; }
    public int MaxSpeedMbps { get; set; }
    public int? MaxSpeedMbps5G { get; set; }
    public int? MaxSpeedMbps6G { get; set; }
    public PcieSlotType? PcieSlotType { get; set; }
    public M2Key? Key { get; set; }
    public M2FormFactor? M2FormFactor { get; set; }
    public UsbVersion? UsbVersion { get; set; }
    public UsbType? UsbType { get; set; }

    protected WirelessNetworkAdapter()
    {
    }

    public WirelessNetworkAdapter(
        string name,
        Guid manufacturerId,
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion,
        PcieSlotType? pcieSlotType = null,
        M2Key? m2Key = null,
        M2FormFactor? m2FormFactor = null,
        UsbVersion? usbVersion = null,
        UsbType? usbType = null)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(
            wifiStandard,
            hostInterface,
            maxSpeedMbps,
            maxSpeedMbps5G,
            maxSpeedMbps6G,
            bluetoothVersion,
            pcieSlotType,
            m2Key,
            m2FormFactor,
            usbVersion,
            usbType);
    }

    public void UpdateSpecs(
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion,
        PcieSlotType? pcieSlotType = null,
        M2Key? m2Key = null,
        M2FormFactor? m2FormFactor = null,
        UsbVersion? usbVersion = null,
        UsbType? usbType = null)
    {
        SetSpecs(
            wifiStandard,
            hostInterface,
            maxSpeedMbps,
            maxSpeedMbps5G,
            maxSpeedMbps6G,
            bluetoothVersion,
            pcieSlotType,
            m2Key,
            m2FormFactor,
            usbVersion,
            usbType);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion,
        PcieSlotType? pcieSlotType,
        M2Key? m2Key,
        M2FormFactor? m2FormFactor,
        UsbVersion? usbVersion,
        UsbType? usbType)
    {
        if (!Enum.IsDefined(wifiStandard))
            throw new ArgumentException("Wifi Standard is invalid");

        if (!Enum.IsDefined(hostInterface))
            throw new ArgumentException("Host interface is invalid");

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps);

        if (maxSpeedMbps5G.HasValue)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps5G.Value);

        if (maxSpeedMbps6G.HasValue)
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxSpeedMbps6G.Value);

        if (bluetoothVersion.HasValue && !Enum.IsDefined(bluetoothVersion.Value))
            throw new ArgumentException("Bluetooth version is invalid");

        switch (hostInterface)
        {
            case WirelessHostInterface.M2:
                if (m2Key is not M2Key.E)
                    throw new ArgumentException("M.2 wireless adapters must use E-key.");
                if (m2FormFactor is null || !Enum.IsDefined(m2FormFactor.Value))
                    throw new ArgumentException("M.2 form factor is required for M.2 wireless adapters.");
                if (pcieSlotType.HasValue)
                    throw new ArgumentException("PCIe slot type is not valid for M.2 wireless adapters.");
                if (usbVersion.HasValue || usbType.HasValue)
                    throw new ArgumentException("USB fields are not valid for M.2 wireless adapters.");
                break;
            case WirelessHostInterface.Pcie:
                if (pcieSlotType is null || !Enum.IsDefined(pcieSlotType.Value))
                    throw new ArgumentException("PCIe slot type is required for PCIe wireless adapters.");
                if (m2Key.HasValue || m2FormFactor.HasValue)
                    throw new ArgumentException("M.2 key and form factor are not valid for PCIe wireless adapters.");
                if (usbVersion.HasValue || usbType.HasValue)
                    throw new ArgumentException("USB fields are not valid for PCIe wireless adapters.");
                break;
            case WirelessHostInterface.Usb:
                if (usbVersion is null || !Enum.IsDefined(usbVersion.Value))
                    throw new ArgumentException("USB version is required for USB wireless adapters.");
                if (usbType is null || !Enum.IsDefined(usbType.Value))
                    throw new ArgumentException("USB type is required for USB wireless adapters.");
                if (pcieSlotType.HasValue || m2Key.HasValue || m2FormFactor.HasValue)
                    throw new ArgumentException("USB wireless adapters cannot have PCIe or M.2 fields.");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(hostInterface));
        }

        WifiStandard = wifiStandard;
        HostInterface = hostInterface;
        MaxSpeedMbps = maxSpeedMbps;
        MaxSpeedMbps5G = maxSpeedMbps5G;
        MaxSpeedMbps6G = maxSpeedMbps6G;
        BluetoothVersion = bluetoothVersion;
        PcieSlotType = pcieSlotType;
        Key = m2Key;
        M2FormFactor = m2FormFactor;
        UsbVersion = usbVersion;
        UsbType = usbType;
    }
}
