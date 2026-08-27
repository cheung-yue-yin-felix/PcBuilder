using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class MotherboardTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();
    private static readonly Guid SocketId = Guid.NewGuid();
    private static readonly Guid ChipsetId = Guid.NewGuid();

    [Fact]
    public void Pcie_m2_and_usb_collections_reject_duplicates()
    {
        var board = Create();
        var pcie = new MotherboardPcie(board.Id, PcieSlotType.X16, PcieSlotLane.X16, PcieGeneration.Gen4, 1);
        var m2 = new MotherboardM2(board.Id, M2Key.M, PcieGeneration.Gen4, 1, true);
        var usb = new MotherboardUsb(board.Id, UsbVersion.Usb32Gen2, UsbType.TypeA, 4);

        board.AddPcieSlot(pcie);
        board.AddM2Slot(m2);
        board.AddUsbPort(usb);

        var dupPcie = () => board.AddPcieSlot(new MotherboardPcie(board.Id, PcieSlotType.X16, PcieSlotLane.X16, PcieGeneration.Gen4, 2));
        var dupM2 = () => board.AddM2Slot(new MotherboardM2(board.Id, M2Key.M, PcieGeneration.Gen4, 2, false));
        var dupUsb = () => board.AddUsbPort(new MotherboardUsb(board.Id, UsbVersion.Usb32Gen2, UsbType.TypeA, 2));

        dupPcie.Should().Throw<ArgumentException>();
        dupM2.Should().Throw<ArgumentException>();
        dupUsb.Should().Throw<ArgumentException>();

        board.RemovePcieSlot(pcie);
        board.RemoveM2Slot(m2);
        board.RemoveUsbPort(usb);
        board.PcieSlots.Should().BeEmpty();
    }

    [Fact]
    public void Memory_compatibility_checks_generation_form_factor_and_size()
    {
        var board = Create();
        var ok = CreateRam(DdrGeneration.Ddr5, RamFormFactor.UDimm, 16, 32, 2);
        var wrongGen = CreateRam(DdrGeneration.Ddr4, RamFormFactor.UDimm, 16, 32, 2);
        var tooManyModules = CreateRam(DdrGeneration.Ddr5, RamFormFactor.UDimm, 16, 64, 8);

        board.CheckMemoryCompatibility(ok).Should().BeTrue();
        board.CheckMemoryCompatibility(wrongGen).Should().BeFalse();
        board.CheckMemoryCompatibility(tooManyModules).Should().BeFalse();
    }

    [Fact]
    public void Cpu_compatibility_uses_socket_and_chipset_support()
    {
        var board = Create();
        var cpu = new Cpu("7800X3D", ManufacturerId, SocketId, Guid.NewGuid(), 128, false, false, 120, 120);

        board.CheckCpuCompatibility(cpu).Reason.Should().Be(CompatibilityReason.ChipsetNotSupported);

        cpu.AddSupportedChipset(new CpuSupportChipset(cpu.Id, ChipsetId, requiresBiosUpdate: true));
        board.CheckCpuCompatibility(cpu).Status.Should().Be(PartsCompatibility.CompatibleActionRequired);

        cpu.RemoveSupportedChipset(cpu.SupportedChipsets.Single());
        cpu.AddSupportedChipset(new CpuSupportChipset(cpu.Id, ChipsetId));
        board.CheckCpuCompatibility(cpu).Status.Should().Be(PartsCompatibility.Compatible);

        var wrongSocket = new Cpu("i9", ManufacturerId, Guid.NewGuid(), Guid.NewGuid(), 128, false, false, 125, 125);
        board.CheckCpuCompatibility(wrongSocket).Reason.Should().Be(CompatibilityReason.SocketMismatch);
    }

    [Fact]
    public void Graphics_card_compatibility_requires_x16_and_may_reduce_generation()
    {
        var board = Create();
        var gpu = CreateGpu(PcieGeneration.Gen5);

        board.CheckGraphicsCardCompatibility(gpu).Reason.Should().Be(CompatibilityReason.NotEnoughPcieSlots);

        board.AddPcieSlot(new MotherboardPcie(board.Id, PcieSlotType.X16, PcieSlotLane.X16, PcieGeneration.Gen4, 1));
        var reduced = board.CheckGraphicsCardCompatibility(gpu);
        reduced.Status.Should().Be(PartsCompatibility.CompatibleReduced);
        reduced.Reason.Should().Be(CompatibilityReason.PcieGenerationReduced);
    }

    [Fact]
    public void E_key_m2_slot_cannot_support_sata()
    {
        var act = () => new MotherboardM2(Guid.NewGuid(), M2Key.E, PcieGeneration.Gen4, 1, supportsSata: true);

        act.Should().Throw<ArgumentException>().WithParameterName("supportsSata");
    }

    private static Motherboard Create() =>
        new(ManufacturerId, "B650", SocketId, ChipsetId, 4, 128, 48, 4, 4, 2, 244, 305,
            DdrGeneration.Ddr5, RamFormFactor.UDimm, MbFormFactor.Atx, false, false);

    private static Ram CreateRam(DdrGeneration ddr, RamFormFactor form, int perStick, int total, int modules) =>
        new("Kit", ManufacturerId, "Black", ddr, form, RamRank.DualRank, perStick, total, modules, 6000, 40);

    private static GraphicsCard CreateGpu(PcieGeneration gen) =>
        new("RTX 4070", ManufacturerId, Guid.NewGuid(), 12, 2, gen, 240, 120, 50, 200, PsuCableType.Pcie6Plus2Pin, 2);
}
