using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class NetworkAdapterTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Wired_pcie_requires_slot_and_rejects_usb_fields()
    {
        var adapter = new WiredNetworkAdapter("I225-V", ManufacturerId, WiredHostInterface.Pcie, 2500,
            pcieSlotType: PcieSlotType.X1);
        adapter.PcieSlotType.Should().Be(PcieSlotType.X1);

        var missingSlot = () => new WiredNetworkAdapter("NIC", ManufacturerId, WiredHostInterface.Pcie, 1000);
        var usbOnPcie = () => new WiredNetworkAdapter("NIC", ManufacturerId, WiredHostInterface.Pcie, 1000,
            UsbVersion.Usb32Gen1, UsbType.TypeA, PcieSlotType.X1);

        missingSlot.Should().Throw<ArgumentException>();
        usbOnPcie.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Wired_usb_requires_usb_fields()
    {
        var adapter = new WiredNetworkAdapter("USB NIC", ManufacturerId, WiredHostInterface.Usb, 1000,
            UsbVersion.Usb32Gen1, UsbType.TypeA);
        adapter.UsbType.Should().Be(UsbType.TypeA);

        var missingUsb = () => new WiredNetworkAdapter("USB NIC", ManufacturerId, WiredHostInterface.Usb, 1000);
        missingUsb.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Wireless_m2_must_be_e_key()
    {
        var ok = new WirelessNetworkAdapter("AX210", ManufacturerId, WifiStandard.Wifi6E, WirelessHostInterface.M2,
            2400, null, null, BluetoothVersion.V5Point2, m2Key: M2Key.E, m2FormFactor: M2FormFactor.M22230);
        ok.Key.Should().Be(M2Key.E);

        var wrongKey = () => new WirelessNetworkAdapter("AX210", ManufacturerId, WifiStandard.Wifi6E,
            WirelessHostInterface.M2, 2400, null, null, null, m2Key: M2Key.M, m2FormFactor: M2FormFactor.M22230);
        wrongKey.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Wireless_usb_rejects_pcie_and_m2_fields()
    {
        var ok = new WirelessNetworkAdapter("USB WiFi", ManufacturerId, WifiStandard.Wifi6, WirelessHostInterface.Usb,
            1200, null, null, null, usbVersion: UsbVersion.Usb32Gen1, usbType: UsbType.TypeA);
        ok.HostInterface.Should().Be(WirelessHostInterface.Usb);

        var mixed = () => new WirelessNetworkAdapter("USB WiFi", ManufacturerId, WifiStandard.Wifi6,
            WirelessHostInterface.Usb, 1200, null, null, null, pcieSlotType: PcieSlotType.X1,
            usbVersion: UsbVersion.Usb32Gen1, usbType: UsbType.TypeA);
        mixed.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Graphics_card_rejects_empty_gpu_and_zero_memory()
    {
        var card = new GraphicsCard("RTX 4070", ManufacturerId, Guid.NewGuid(), 12, 2, PcieGeneration.Gen4,
            240, 120, 50, 200, PsuCableType.Pcie6Plus2Pin, 2);
        card.VideoMemoryGb.Should().Be(12);

        var emptyGpu = () => new GraphicsCard("GPU", ManufacturerId, Guid.Empty, 12, 2, PcieGeneration.Gen4,
            240, 120, 50, 200, PsuCableType.Pcie6Plus2Pin, 2);
        emptyGpu.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void M2_module_fits_matching_and_bm_slots()
    {
        M2Key.M.IsSlotKey().Should().BeTrue();
        M2Key.BM.IsSlotKey().Should().BeFalse();
        M2Key.M.FitsSlot(M2Key.M).Should().BeTrue();
        M2Key.BM.FitsSlot(M2Key.M).Should().BeTrue();
        M2Key.BM.FitsSlot(M2Key.E).Should().BeFalse();
    }
}
