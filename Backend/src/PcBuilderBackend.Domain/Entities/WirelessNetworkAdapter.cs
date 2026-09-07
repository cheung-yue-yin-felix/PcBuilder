using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.Entities;

public class WirelessNetworkAdapter : ProductEntity
{
    public WifiStandard WifiStandard { get; private set; }
    public BluetoothVersion? BluetoothVersion { get; private set; }
    public WirelessHostInterface HostInterface { get; private set; }
    public int MaxSpeedMbps { get; private set; }
    public int? MaxSpeedMbps5G { get; private set; }
    public int? MaxSpeedMbps6G { get; private set; }
    public PcieSlotType? PcieSlotType { get; private set; }
    public M2Key? Key { get; private set; }
    public M2FormFactor? M2FormFactor { get; private set; }
    public UsbVersion? UsbVersion { get; private set; }
    public UsbType? UsbType { get; private set; }

    protected WirelessNetworkAdapter()
    {
    }

    public WirelessNetworkAdapter(string name, Guid manufacturerId, WirelessNetworkAdapterSpecs specs)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSpecs(specs);
    }

    public void UpdateSpecs(WirelessNetworkAdapterSpecs specs)
    {
        SetSpecs(specs);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(WirelessNetworkAdapterSpecs specs)
    {
        ValidateCommonSpecs(
            specs.WifiStandard,
            specs.HostInterface,
            specs.MaxSpeedMbps,
            specs.MaxSpeedMbps5G,
            specs.MaxSpeedMbps6G,
            specs.BluetoothVersion);
        ValidateHostInterface(
            specs.HostInterface,
            specs.PcieSlotType,
            specs.M2Key,
            specs.M2FormFactor,
            specs.UsbVersion,
            specs.UsbType);

        WifiStandard = specs.WifiStandard;
        HostInterface = specs.HostInterface;
        MaxSpeedMbps = specs.MaxSpeedMbps;
        MaxSpeedMbps5G = specs.MaxSpeedMbps5G;
        MaxSpeedMbps6G = specs.MaxSpeedMbps6G;
        BluetoothVersion = specs.BluetoothVersion;
        PcieSlotType = specs.PcieSlotType;
        Key = specs.M2Key;
        M2FormFactor = specs.M2FormFactor;
        UsbVersion = specs.UsbVersion;
        UsbType = specs.UsbType;
    }

    private static void ValidateCommonSpecs(
        WifiStandard wifiStandard,
        WirelessHostInterface hostInterface,
        int maxSpeedMbps,
        int? maxSpeedMbps5G,
        int? maxSpeedMbps6G,
        BluetoothVersion? bluetoothVersion)
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
    }

    private static void ValidateHostInterface(
        WirelessHostInterface hostInterface,
        PcieSlotType? pcieSlotType,
        M2Key? m2Key,
        M2FormFactor? m2FormFactor,
        UsbVersion? usbVersion,
        UsbType? usbType)
    {
        switch (hostInterface)
        {
            case WirelessHostInterface.M2:
                ValidateM2Interface(pcieSlotType, m2Key, m2FormFactor, usbVersion, usbType);
                return;
            case WirelessHostInterface.Pcie:
                ValidatePcieInterface(pcieSlotType, m2Key, m2FormFactor, usbVersion, usbType);
                return;
            case WirelessHostInterface.Usb:
                ValidateUsbInterface(pcieSlotType, m2Key, m2FormFactor, usbVersion, usbType);
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(hostInterface));
        }
    }

    private static void ValidateM2Interface(
        PcieSlotType? pcieSlotType,
        M2Key? m2Key,
        M2FormFactor? m2FormFactor,
        UsbVersion? usbVersion,
        UsbType? usbType)
    {
        if (m2Key is not M2Key.E)
            throw new ArgumentException("M.2 wireless adapters must use E-key.");
        if (m2FormFactor is null || !Enum.IsDefined(m2FormFactor.Value))
            throw new ArgumentException("M.2 form factor is required for M.2 wireless adapters.");
        if (pcieSlotType.HasValue)
            throw new ArgumentException("PCIe slot type is not valid for M.2 wireless adapters.");
        if (usbVersion.HasValue || usbType.HasValue)
            throw new ArgumentException("USB fields are not valid for M.2 wireless adapters.");
    }

    private static void ValidatePcieInterface(
        PcieSlotType? pcieSlotType,
        M2Key? m2Key,
        M2FormFactor? m2FormFactor,
        UsbVersion? usbVersion,
        UsbType? usbType)
    {
        if (pcieSlotType is null || !Enum.IsDefined(pcieSlotType.Value))
            throw new ArgumentException("PCIe slot type is required for PCIe wireless adapters.");
        if (m2Key.HasValue || m2FormFactor.HasValue)
            throw new ArgumentException("M.2 key and form factor are not valid for PCIe wireless adapters.");
        if (usbVersion.HasValue || usbType.HasValue)
            throw new ArgumentException("USB fields are not valid for PCIe wireless adapters.");
    }

    private static void ValidateUsbInterface(
        PcieSlotType? pcieSlotType,
        M2Key? m2Key,
        M2FormFactor? m2FormFactor,
        UsbVersion? usbVersion,
        UsbType? usbType)
    {
        if (usbVersion is null || !Enum.IsDefined(usbVersion.Value))
            throw new ArgumentException("USB version is required for USB wireless adapters.");
        if (usbType is null || !Enum.IsDefined(usbType.Value))
            throw new ArgumentException("USB type is required for USB wireless adapters.");
        if (pcieSlotType.HasValue || m2Key.HasValue || m2FormFactor.HasValue)
            throw new ArgumentException("USB wireless adapters cannot have PCIe or M.2 fields.");
    }
}
