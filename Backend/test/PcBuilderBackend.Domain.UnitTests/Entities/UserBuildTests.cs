using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Entities;

public class UserBuildTests
{
    [Fact]
    public void Constructor_sets_identity_trims_fields_and_components()
    {
        var chassisId = Guid.NewGuid();
        var motherboardId = Guid.NewGuid();
        var cpuId = Guid.NewGuid();
        var coolerId = Guid.NewGuid();
        var ramKitId = Guid.NewGuid();
        var gpuId = Guid.NewGuid();
        var psuId = Guid.NewGuid();

        var build = new PcBuild(
            "  Gaming Rig  ",
            "  fast  ",
            chassisId,
            motherboardId,
            cpuId,
            coolerId,
            ramKitId,
            gpuId,
            psuId);

        build.Name.Should().Be("Gaming Rig");
        build.Description.Should().Be("fast");
        build.ChassisId.Should().Be(chassisId);
        build.MotherboardId.Should().Be(motherboardId);
        build.CpuId.Should().Be(cpuId);
        build.CpuCoolerId.Should().Be(coolerId);
        build.RamKitId.Should().Be(ramKitId);
        build.GraphicsCardId.Should().Be(gpuId);
        build.PsuId.Should().Be(psuId);
    }

    [Fact]
    public void Constructor_treats_empty_optional_ids_as_null()
    {
        var build = CreateBuild(cpuCoolerId: Guid.Empty, graphicsCardId: Guid.Empty);

        build.CpuCoolerId.Should().BeNull();
        build.GraphicsCardId.Should().BeNull();
    }

    [Fact]
    public void Constructor_rejects_empty_name_and_overlong_description()
    {
        var emptyName = () => CreateBuild(name: "");
        var longDescription = () => CreateBuild(description: new string('x', 501));

        emptyName.Should().Throw<ArgumentException>().WithParameterName("name");
        longDescription.Should().Throw<ArgumentException>().WithParameterName("description");
    }

    [Fact]
    public void Constructor_rejects_empty_required_component_ids()
    {
        var emptyChassis = () => CreateBuild(chassisId: Guid.Empty);
        var emptyMotherboard = () => CreateBuild(motherboardId: Guid.Empty);
        var emptyCpu = () => CreateBuild(cpuId: Guid.Empty);
        var emptyRam = () => CreateBuild(ramKitId: Guid.Empty);
        var emptyPsu = () => CreateBuild(psuId: Guid.Empty);

        emptyChassis.Should().Throw<ArgumentException>().WithParameterName("chassisId");
        emptyMotherboard.Should().Throw<ArgumentException>().WithParameterName("motherboardId");
        emptyCpu.Should().Throw<ArgumentException>().WithParameterName("cpuId");
        emptyRam.Should().Throw<ArgumentException>().WithParameterName("ramKitId");
        emptyPsu.Should().Throw<ArgumentException>().WithParameterName("psuId");
    }

    [Fact]
    public void Update_changes_fields_and_clears_optional_components()
    {
        var build = CreateBuild(cpuCoolerId: Guid.NewGuid(), graphicsCardId: Guid.NewGuid());
        var chassisId = Guid.NewGuid();
        var motherboardId = Guid.NewGuid();
        var cpuId = Guid.NewGuid();
        var ramKitId = Guid.NewGuid();

        var psuId = Guid.NewGuid();
        build.Update("New", "desc", chassisId, motherboardId, cpuId, null, ramKitId, null, psuId);

        build.Name.Should().Be("New");
        build.Description.Should().Be("desc");
        build.ChassisId.Should().Be(chassisId);
        build.MotherboardId.Should().Be(motherboardId);
        build.CpuId.Should().Be(cpuId);
        build.RamKitId.Should().Be(ramKitId);
        build.CpuCoolerId.Should().BeNull();
        build.GraphicsCardId.Should().BeNull();
        build.PsuId.Should().Be(psuId);
        build.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void AddStorageDevice_creates_row_and_increments_quantity_for_same_part()
    {
        var build = CreateBuild();
        var partId = Guid.NewGuid();

        build.AddStorageDevice(partId);
        build.AddStorageDevice(partId, 2);

        var part = build.StorageDevices.Should().ContainSingle().Subject;
        part.PartId.Should().Be(partId);
        part.Type.Should().Be(PcBuildPartType.StorageDrive);
        part.PcBuildId.Should().Be(build.Id);
        part.Quantity.Should().Be(3);
    }

    [Fact]
    public void RemoveStorageDevice_decrements_then_removes_row()
    {
        var build = CreateBuild();
        var partId = Guid.NewGuid();
        build.AddStorageDevice(partId, 3);

        build.RemoveStorageDevice(partId);
        build.StorageDevices.Should().ContainSingle().Which.Quantity.Should().Be(2);

        build.RemoveStorageDevice(partId, 2);
        build.StorageDevices.Should().BeEmpty();
    }

    [Fact]
    public void RemoveStorageDevice_rejects_missing_part_or_excess_quantity()
    {
        var build = CreateBuild();
        var partId = Guid.NewGuid();
        build.AddStorageDevice(partId, 2);

        var missing = () => build.RemoveStorageDevice(Guid.NewGuid());
        var excess = () => build.RemoveStorageDevice(partId, 3);

        missing.Should().Throw<ArgumentException>().WithParameterName("storageDeviceId");
        excess.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("quantity");
    }

    [Fact]
    public void AddPart_rejects_empty_id_and_non_positive_quantity()
    {
        var build = CreateBuild();

        var emptyId = () => build.AddChassisFan(Guid.Empty);
        var zeroQty = () => build.AddChassisFan(Guid.NewGuid(), 0);

        emptyId.Should().Throw<ArgumentException>().WithParameterName("chassisFanId");
        zeroQty.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("quantity");
    }

    private static PcBuild CreateBuild(
        string name = "Build",
        string? description = null,
        Guid? chassisId = null,
        Guid? motherboardId = null,
        Guid? cpuId = null,
        Guid? cpuCoolerId = null,
        Guid? ramKitId = null,
        Guid? graphicsCardId = null,
        Guid? psuId = null)
    {
        return new PcBuild(
            name,
            description,
            chassisId ?? Guid.NewGuid(),
            motherboardId ?? Guid.NewGuid(),
            cpuId ?? Guid.NewGuid(),
            cpuCoolerId,
            ramKitId ?? Guid.NewGuid(),
            graphicsCardId,
            psuId ?? Guid.NewGuid());
    }
}
