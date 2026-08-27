using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class StorageAndCoolerTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Storage_hdd_requires_rpm_and_hdd_media()
    {
        var ok = new StorageDrive("HDD", ManufacturerId, StorageMedia.Hdd, StorageInterface.Sata,
            StorageFormFactor.Sata35, 4000, rpm: 7200);
        ok.IsM2.Should().BeFalse();
        ok.Rpm.Should().Be(7200);

        var ssdOnHddForm = () => new StorageDrive("Bad", ManufacturerId, StorageMedia.Ssd, StorageInterface.Sata,
            StorageFormFactor.Sata35, 1000);
        var missingRpm = () => new StorageDrive("Bad", ManufacturerId, StorageMedia.Hdd, StorageInterface.Sata,
            StorageFormFactor.Sata35, 1000);

        ssdOnHddForm.Should().Throw<ArgumentException>().WithParameterName("storageMedia");
        missingRpm.Should().Throw<ArgumentException>().WithParameterName("rpm");
    }

    [Fact]
    public void Storage_ssd_requires_pcie_generation_and_exposes_m2_key()
    {
        var nvme = new StorageDrive("990 PRO", ManufacturerId, StorageMedia.Ssd, StorageInterface.Nvme,
            StorageFormFactor.M22280, 2000, PcieGeneration.Gen4);
        nvme.IsM2.Should().BeTrue();
        nvme.ModuleKey.Should().Be(M2Key.M);
        nvme.M2FormFactor.Should().Be(M2FormFactor.M22280);

        var missingGen = () => new StorageDrive("SSD", ManufacturerId, StorageMedia.Ssd, StorageInterface.Nvme,
            StorageFormFactor.M22280, 1000);
        missingGen.Should().Throw<ArgumentException>().WithParameterName("pcieGeneration");
    }

    [Fact]
    public void Air_cooler_requires_heights_and_checks_cpu_tdp_and_ram_clearance()
    {
        var cooler = new CpuCooler(ManufacturerId, "NH-D15", 150, CpuCoolerType.Air, 165, 32, null);
        var socketId = Guid.NewGuid();
        cooler.AddCpuCoolerSocket(new CpuCoolerSocket(cooler.Id, socketId));

        var cpuOk = new Cpu("CPU", ManufacturerId, socketId, Guid.NewGuid(), 128, false, false, 120, 120);
        var cpuHot = new Cpu("CPU", ManufacturerId, socketId, Guid.NewGuid(), 128, false, false, 200, 200);
        var otherSocket = new Cpu("CPU", ManufacturerId, Guid.NewGuid(), Guid.NewGuid(), 128, false, false, 65, 65);

        cooler.CheckCompatibility(cpuOk).Status.Should().Be(PartsCompatibility.Compatible);
        cooler.CheckCompatibility(cpuHot).Reason.Should().Be(CompatibilityReason.ExceedsThermalDesignPower);
        cooler.CheckCompatibility(otherSocket).Reason.Should().Be(CompatibilityReason.MissingCpuCoolerSocket);

        var shortRam = new Ram("Low", ManufacturerId, "Black", DdrGeneration.Ddr5, RamFormFactor.UDimm,
            RamRank.SingleRank, 16, 32, 2, 6000, 30);
        var tallRam = new Ram("Tall", ManufacturerId, "Black", DdrGeneration.Ddr5, RamFormFactor.UDimm,
            RamRank.SingleRank, 16, 32, 2, 6000, 50);
        cooler.CheckCompatibility(shortRam).Status.Should().Be(PartsCompatibility.Compatible);
        cooler.CheckCompatibility(tallRam).Reason.Should().Be(CompatibilityReason.RamHeightExceedsCoolerLimit);
    }

    [Fact]
    public void Liquid_cooler_requires_radiator_length()
    {
        var ok = new CpuCooler(ManufacturerId, "360", 280, CpuCoolerType.Water, null, null, RadiatorLength.Mm360);
        ok.CoolerHeightMm.Should().BeNull();

        var missing = () => new CpuCooler(ManufacturerId, "360", 280, CpuCoolerType.Water, null, null, null);
        missing.Should().Throw<ArgumentException>().WithParameterName("radiatorLength");
    }

    [Fact]
    public void Chassis_fan_rejects_invalid_diameter_and_count()
    {
        var fan = new ChassisFan("LL120", ManufacturerId, FanDiameterMm.Mm120, 3);
        fan.FansCountPerPack.Should().Be(3);

        var badCount = () => new ChassisFan("Fan", ManufacturerId, FanDiameterMm.Mm120, 0);
        badCount.Should().Throw<ArgumentOutOfRangeException>();
    }
}
