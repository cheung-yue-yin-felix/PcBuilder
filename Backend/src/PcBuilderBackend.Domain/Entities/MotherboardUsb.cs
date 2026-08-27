using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardUsb : BaseEntity
{
    public Guid MotherboardId { get; private set; }
    public UsbVersion UsbVersion { get; private set; }
    public UsbType UsbType { get; private set; }
    public int PortCount { get; private set; }

    protected MotherboardUsb() {}

    public MotherboardUsb(Guid motherboardId, UsbVersion usbVersion, UsbType usbType, int portCount)
    {
        SetSpecs(motherboardId, usbVersion, usbType, portCount);
    }

    public void UpdateSpecs(UsbVersion usbVersion, UsbType usbType, int portCount)
    {
        SetSpecs(MotherboardId, usbVersion, usbType, portCount);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid motherboardId, UsbVersion usbVersion, UsbType usbType, int portCount)
    {
        if (motherboardId == Guid.Empty)
            throw new ArgumentException("Motherboard ID cannot be empty.", nameof(motherboardId));

        if (!Enum.IsDefined(usbVersion))
            throw new ArgumentException("USB version is invalid.", nameof(usbVersion));

        if (!Enum.IsDefined(usbType))
            throw new ArgumentException("USB type is invalid.", nameof(usbType));

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(portCount);

        MotherboardId = motherboardId;
        UsbVersion = usbVersion;
        UsbType = usbType;
        PortCount = portCount;
    }
}
