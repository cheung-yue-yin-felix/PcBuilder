using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class RamTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Constructor_trims_color_and_sets_specs()
    {
        var ram = new Ram("Vengeance", ManufacturerId, new RamSpecs
        {
            Color = "  Black  ",
            DdrGeneration = DdrGeneration.Ddr5,
            RamFormFactor = RamFormFactor.UDimm,
            RamRank = RamRank.DualRank,
            MemorySizePerStickGb = 16,
            TotalMemorySizeGb = 32,
            ModulesCount = 2,
            MaxMemorySpeedMts = 6000,
            HeightMm = 40
        });

        ram.Color.Should().Be("Black");
        ram.TotalMemorySizeGb.Should().Be(32);
        ram.ModulesCount.Should().Be(2);
    }

    [Fact]
    public void Constructor_rejects_invalid_size_relationship()
    {
        var act = () => new Ram("Kit", ManufacturerId, new RamSpecs
        {
            Color = "Black",
            DdrGeneration = DdrGeneration.Ddr5,
            RamFormFactor = RamFormFactor.UDimm,
            RamRank = RamRank.SingleRank,
            MemorySizePerStickGb = 32,
            TotalMemorySizeGb = 16,
            ModulesCount = 2,
            MaxMemorySpeedMts = 6000,
            HeightMm = 40
        });

        act.Should().Throw<ArgumentException>().WithMessage("*Total Memory Size*");
    }

    [Fact]
    public void Constructor_rejects_invalid_enums_and_zero_values()
    {
        var badDdr = () => new Ram("Kit", ManufacturerId, new RamSpecs
        {
            Color = "Black",
            DdrGeneration = (DdrGeneration)1,
            RamFormFactor = RamFormFactor.UDimm,
            RamRank = RamRank.SingleRank,
            MemorySizePerStickGb = 16,
            TotalMemorySizeGb = 32,
            ModulesCount = 2,
            MaxMemorySpeedMts = 6000,
            HeightMm = 40
        });
        var noColor = () => new Ram("Kit", ManufacturerId, new RamSpecs
        {
            Color = "",
            DdrGeneration = DdrGeneration.Ddr5,
            RamFormFactor = RamFormFactor.UDimm,
            RamRank = RamRank.SingleRank,
            MemorySizePerStickGb = 16,
            TotalMemorySizeGb = 32,
            ModulesCount = 2,
            MaxMemorySpeedMts = 6000,
            HeightMm = 40
        });

        badDdr.Should().Throw<ArgumentException>();
        noColor.Should().Throw<ArgumentException>();
    }
}
