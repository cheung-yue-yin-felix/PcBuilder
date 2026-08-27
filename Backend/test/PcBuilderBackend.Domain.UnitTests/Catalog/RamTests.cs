using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class RamTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Constructor_trims_color_and_sets_specs()
    {
        var ram = new Ram("Vengeance", ManufacturerId, "  Black  ", DdrGeneration.Ddr5, RamFormFactor.UDimm,
            RamRank.DualRank, 16, 32, 2, 6000, 40);

        ram.Color.Should().Be("Black");
        ram.TotalMemorySizeGb.Should().Be(32);
        ram.ModulesCount.Should().Be(2);
    }

    [Fact]
    public void Constructor_rejects_invalid_size_relationship()
    {
        var act = () => new Ram("Kit", ManufacturerId, "Black", DdrGeneration.Ddr5, RamFormFactor.UDimm,
            RamRank.SingleRank, 32, 16, 2, 6000, 40);

        act.Should().Throw<ArgumentException>().WithMessage("*Total Memory Size*");
    }

    [Fact]
    public void Constructor_rejects_invalid_enums_and_zero_values()
    {
        var badDdr = () => new Ram("Kit", ManufacturerId, "Black", (DdrGeneration)1, RamFormFactor.UDimm,
            RamRank.SingleRank, 16, 32, 2, 6000, 40);
        var noColor = () => new Ram("Kit", ManufacturerId, "", DdrGeneration.Ddr5, RamFormFactor.UDimm,
            RamRank.SingleRank, 16, 32, 2, 6000, 40);

        badDdr.Should().Throw<ArgumentException>();
        noColor.Should().Throw<ArgumentException>();
    }
}
