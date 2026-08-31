using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class ChassisTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Child_collections_reject_duplicates_and_missing_removes()
    {
        var chassis = Create();
        var bay = new ChassisDriveBay(chassis.Id, DriveBayFormFactor.Inch35, 2);
        chassis.AddDriveBay(bay);
        var dup = () => chassis.AddDriveBay(new ChassisDriveBay(chassis.Id, DriveBayFormFactor.Inch35, 1));
        dup.Should().Throw<ArgumentException>();
        chassis.RemoveDriveBay(bay);
        chassis.DriveBays.Should().BeEmpty();

        var mount = new ChassisFanMount(chassis.Id, FanMountLocation.Front, false);
        chassis.AddFanMount(mount);
        var dupMount = () => chassis.AddFanMount(new ChassisFanMount(chassis.Id, FanMountLocation.Front, true));
        dupMount.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Motherboard_compatibility_checks_form_factor_and_size()
    {
        var chassis = Create();
        chassis.AddMbFormFactor(new ChassisMbFormFactor(chassis.Id, MbFormFactor.Atx));
        var board = new Motherboard(ManufacturerId, "B650", Guid.NewGuid(), Guid.NewGuid(), 4, 128, 48, 4, 4, 2,
            244, 244, DdrGeneration.Ddr5, RamFormFactor.UDimm, MbFormFactor.Atx, false, false);
        var itx = new Motherboard(ManufacturerId, "ITX", Guid.NewGuid(), Guid.NewGuid(), 2, 64, 32, 4, 3, 1,
            170, 170, DdrGeneration.Ddr5, RamFormFactor.UDimm, MbFormFactor.Mitx, false, false);

        chassis.CheckMotherboardCompatibility(board).Should().BeTrue();
        chassis.CheckMotherboardCompatibility(itx).Should().BeFalse();
    }

    [Fact]
    public void Storage_compatibility_uses_drive_bays_and_ignores_m2()
    {
        var chassis = Create();
        chassis.AddDriveBay(new ChassisDriveBay(chassis.Id, DriveBayFormFactor.Inch35, 1));
        var hdd = new StorageDrive("HDD", ManufacturerId, StorageMedia.Hdd, StorageInterface.Sata,
            StorageFormFactor.Sata35, 4000, rpm: 7200);
        var nvme = new StorageDrive("SSD", ManufacturerId, StorageMedia.Ssd, StorageInterface.Nvme,
            StorageFormFactor.M22280, 2000, PcieGeneration.Gen4);

        chassis.CheckStorageDriveCompatibility(nvme).Should().BeTrue();
        chassis.CheckStorageDriveCompatibility(hdd).Should().BeTrue();
        chassis.CheckStorageDriveCompatibility([hdd, hdd]).Should().BeFalse();

        chassis.AddDriveBay(new ChassisDriveBay(chassis.Id, DriveBayFormFactor.Inch25, 1));
        var sataSsd = new StorageDrive("MX500", ManufacturerId, StorageMedia.Ssd, StorageInterface.Sata,
            StorageFormFactor.Sata25, 1000);
        chassis.CheckStorageDriveCompatibility(sataSsd).Should().BeTrue();
        chassis.CheckStorageDriveCompatibility([sataSsd, sataSsd]).Should().BeFalse();
    }

    [Fact]
    public void Fan_compatibility_assigns_mount_capacity()
    {
        var chassis = Create();
        var mount = new ChassisFanMount(chassis.Id, FanMountLocation.Front, false);
        mount.AddOption(new ChassisFanMountOption(mount.Id, FanDiameterMm.Mm120, 3));
        chassis.AddFanMount(mount);

        var fan = new ChassisFan("LL120", ManufacturerId, FanDiameterMm.Mm120, 3);
        var tooMany = new ChassisFan("LL120", ManufacturerId, FanDiameterMm.Mm120, 4);

        chassis.CheckFanCompatibility(fan).Should().BeTrue();
        chassis.CheckFanCompatibility(tooMany).Should().BeFalse();
    }

    [Fact]
    public void Air_cooler_height_and_aio_radiator_are_checked()
    {
        var chassis = Create();
        chassis.AddRadiator(new ChassisRadiator(chassis.Id, RadiatorLength.Mm360, RadiatorMountLocation.Top, 1));

        var airOk = new CpuCooler(ManufacturerId, "NH-D15", 220, CpuCoolerType.Air, 150, 35, null);
        var airTall = new CpuCooler(ManufacturerId, "Tall", 220, CpuCoolerType.Air, 180, 35, null);
        var aio = new CpuCooler(ManufacturerId, "360", 280, CpuCoolerType.Water, null, null, RadiatorLength.Mm360);
        var aioWrong = new CpuCooler(ManufacturerId, "240", 250, CpuCoolerType.Water, null, null, RadiatorLength.Mm240);

        chassis.CheckCpuCoolerCompatibility(airOk).Should().BeTrue();
        chassis.CheckCpuCoolerCompatibility(airTall).Should().BeFalse();
        chassis.CheckCpuCoolerCompatibility(aio).Should().BeTrue();
        chassis.CheckCpuCoolerCompatibility(aioWrong).Should().BeFalse();
    }

    [Fact]
    public void Constructor_rejects_non_positive_dimensions()
    {
        var act = () => new Chassis("Case", ManufacturerId, 0, 230, 460, 305, 244, 170, 370, 180);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    private static Chassis Create() =>
        new("4000D", ManufacturerId, 450, 230, 460, 305, 244, 170, 370, 180);
}
