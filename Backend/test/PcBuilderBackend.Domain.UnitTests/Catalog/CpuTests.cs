using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class CpuTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();
    private static readonly Guid SocketId = Guid.NewGuid();
    private static readonly Guid SeriesId = Guid.NewGuid();

    [Fact]
    public void Constructor_sets_specs()
    {
        var cpu = Create();

        cpu.Name.Should().Be("7800X3D");
        cpu.SocketId.Should().Be(SocketId);
        cpu.MaxMemoryGb.Should().Be(128);
        cpu.PowerConsumptionWatts.Should().Be(120);
    }

    [Fact]
    public void Constructor_rejects_empty_socket_series_and_non_positive_limits()
    {
        var emptySocket = () => new Cpu("x", ManufacturerId, Guid.Empty, SeriesId, 128, false, false, 120, 120);
        var emptySeries = () => new Cpu("x", ManufacturerId, SocketId, Guid.Empty, 128, false, false, 120, 120);
        var badMemory = () => new Cpu("x", ManufacturerId, SocketId, SeriesId, 0, false, false, 120, 120);
        var badTdp = () => new Cpu("x", ManufacturerId, SocketId, SeriesId, 128, false, false, 0, 120);

        emptySocket.Should().Throw<ArgumentException>().WithParameterName("socketId");
        emptySeries.Should().Throw<ArgumentException>().WithParameterName("seriesId");
        badMemory.Should().Throw<ArgumentException>().WithParameterName("maxMemoryGb");
        badTdp.Should().Throw<ArgumentException>().WithParameterName("thermalDesignPower");
    }

    [Fact]
    public void Ram_compat_add_and_remove_guard_duplicates()
    {
        var cpu = Create();
        var compat = new CpuRamCompat(cpu.Id, DdrGeneration.Ddr5, 2, RamRank.DualRank, 6000);
        cpu.AddRamCompat(compat);

        var duplicate = () => cpu.AddRamCompat(new CpuRamCompat(cpu.Id, DdrGeneration.Ddr5, 2, RamRank.DualRank, 5600));
        duplicate.Should().Throw<ArgumentException>().WithMessage("*already exists*");

        cpu.RemoveRamCompat(compat);
        cpu.RamCompats.Should().BeEmpty();

        var missing = () => cpu.RemoveRamCompat(compat);
        missing.Should().Throw<ArgumentException>().WithMessage("*does not exist*");
    }

    [Fact]
    public void Supported_chipset_add_and_remove_guard_duplicates()
    {
        var cpu = Create();
        var chipsetId = Guid.NewGuid();
        var support = new CpuSupportChipset(cpu.Id, chipsetId);
        cpu.AddSupportedChipset(support);

        var duplicate = () => cpu.AddSupportedChipset(new CpuSupportChipset(cpu.Id, chipsetId));
        duplicate.Should().Throw<ArgumentException>();

        cpu.RemoveSupportedChipset(support);
        cpu.SupportedChipsets.Should().BeEmpty();
    }

    [Fact]
    public void Memory_compatibility_matches_config_and_speed()
    {
        var cpu = Create();
        cpu.AddRamCompat(new CpuRamCompat(cpu.Id, DdrGeneration.Ddr5, 2, RamRank.DualRank, 6000));

        var matching = CreateRam(DdrGeneration.Ddr5, 2, RamRank.DualRank, 5600);
        var tooFast = CreateRam(DdrGeneration.Ddr5, 2, RamRank.DualRank, 7200);
        var wrongGen = CreateRam(DdrGeneration.Ddr4, 2, RamRank.DualRank, 3200);

        cpu.CheckMemoryCompatibility(matching).Status.Should().Be(PartsCompatibility.Compatible);
        cpu.CheckMemoryCompatibility(tooFast).Status.Should().Be(PartsCompatibility.CompatibleReduced);
        cpu.CheckMemoryCompatibility(tooFast).Reason.Should().Be(CompatibilityReason.MemorySpeedExceedsCpuSupport);
        cpu.CheckMemoryCompatibility(wrongGen).Status.Should().Be(PartsCompatibility.Incompatible);
        cpu.CheckMemoryCompatibility(wrongGen).Reason.Should().Be(CompatibilityReason.NoMatchingRamConfig);
    }

    private static Cpu Create() =>
        new("7800X3D", ManufacturerId, SocketId, SeriesId, 128, false, false, 120, 120);

    private static Ram CreateRam(DdrGeneration ddr, int modules, RamRank rank, int speed) =>
        new("Kit", ManufacturerId, "Black", ddr, RamFormFactor.UDimm, rank, 16, 16 * modules, modules, speed, 40);
}
