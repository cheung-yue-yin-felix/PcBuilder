using FluentAssertions;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Domain.UnitTests.Entities;

public class ManufacturerTests
{
    [Fact]
    public void Constructor_trims_name()
    {
        var manufacturer = new Manufacturer("  Corsair  ");

        manufacturer.Name.Should().Be("Corsair");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_rejects_missing_name(string? name)
    {
        var act = () => new Manufacturer(name!);

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_rejects_name_over_200_characters()
    {
        var act = () => new Manufacturer(new string('A', 201));

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Rename_updates_name_and_timestamp()
    {
        var manufacturer = new Manufacturer("Corsair");

        manufacturer.Rename("Seasonic");

        manufacturer.Name.Should().Be("Seasonic");
        manufacturer.UpdatedAtUtc.Should().NotBeNull();
    }
}
