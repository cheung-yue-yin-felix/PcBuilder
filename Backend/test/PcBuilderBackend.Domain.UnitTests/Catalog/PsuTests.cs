using FluentAssertions;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Domain.UnitTests.Catalog;

public class PsuTests
{
    private static readonly Guid ManufacturerId = Guid.NewGuid();

    [Fact]
    public void Constructor_sets_trimmed_name_and_specs()
    {
        var psu = Create("  RM850x  ");

        psu.Name.Should().Be("RM850x");
        psu.ManufacturerId.Should().Be(ManufacturerId);
        psu.Wattage.Should().Be(850);
        psu.Modularity.Should().Be(PsuModularity.FullModular);
        psu.FormFactor.Should().Be(PsuFormFactor.Atx);
        psu.LengthMm.Should().Be(160);
        psu.WidthMm.Should().Be(150);
        psu.HeightMm.Should().Be(86);
        psu.IsActive.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_rejects_missing_name(string? name)
    {
        var act = () => Create(name!);

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_rejects_name_longer_than_200_characters()
    {
        var act = () => Create(new string('A', 201));

        act.Should().Throw<ArgumentException>().WithParameterName("name");
    }

    [Fact]
    public void Constructor_rejects_empty_manufacturer()
    {
        var act = () => new Psu("RM850x", Guid.Empty, new PsuSpecs
        {
            Wattage = 850,
            Modularity = PsuModularity.FullModular,
            FormFactor = PsuFormFactor.Atx,
            LengthMm = 160,
            WidthMm = 150,
            HeightMm = 86
        });

        act.Should().Throw<ArgumentException>().WithParameterName("manufacturerId");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_rejects_non_positive_wattage(int wattage)
    {
        var act = () => Create(wattage: wattage);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Constructor_rejects_invalid_modularity()
    {
        var act = () => Create(modularity: (PsuModularity)99);

        act.Should().Throw<ArgumentException>().WithMessage("*Modularity*");
    }

    [Fact]
    public void Constructor_rejects_invalid_form_factor()
    {
        var act = () => Create(formFactor: (PsuFormFactor)0);

        act.Should().Throw<ArgumentException>().WithMessage("*Form factor*");
    }

    [Theory]
    [InlineData(0, 150, 86)]
    [InlineData(160, 0, 86)]
    [InlineData(160, 150, -1)]
    public void Constructor_rejects_non_positive_dimensions(decimal length, decimal width, decimal height)
    {
        var act = () => Create(lengthMm: length, widthMm: width, heightMm: height);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void UpdateSpecs_replaces_values_and_sets_timestamp()
    {
        var psu = Create();

        psu.UpdateSpecs(new PsuSpecs
        {
            Wattage = 750,
            Modularity = PsuModularity.SemiModular,
            FormFactor = PsuFormFactor.Sfx,
            LengthMm = 100,
            WidthMm = 125,
            HeightMm = 63.5m
        });

        psu.Wattage.Should().Be(750);
        psu.Modularity.Should().Be(PsuModularity.SemiModular);
        psu.FormFactor.Should().Be(PsuFormFactor.Sfx);
        psu.LengthMm.Should().Be(100);
        psu.WidthMm.Should().Be(125);
        psu.HeightMm.Should().Be(63.5m);
        psu.UpdatedAtUtc.Should().NotBeNull();
    }

    [Fact]
    public void Rename_and_UpdateManufacturer_change_identity_fields()
    {
        var psu = Create();
        var nextManufacturer = Guid.NewGuid();

        psu.Rename("SF750");
        psu.UpdateManufacturer(nextManufacturer);

        psu.Name.Should().Be("SF750");
        psu.ManufacturerId.Should().Be(nextManufacturer);
    }

    [Fact]
    public void AddCable_adds_unique_cable_type()
    {
        var psu = Create();
        var cable = new PsuCable(psu.Id, PsuCableType.Motherboard24Pin, 1, 1);

        psu.AddCable(cable);

        psu.Cables.Should().ContainSingle().Which.Should().BeSameAs(cable);
    }

    [Fact]
    public void AddCable_rejects_duplicate_type()
    {
        var psu = Create();
        psu.AddCable(new PsuCable(psu.Id, PsuCableType.Sata, 2, 4));

        var act = () => psu.AddCable(new PsuCable(psu.Id, PsuCableType.Sata, 1, 1));

        act.Should().Throw<ArgumentException>().WithMessage("*already added*");
    }

    [Fact]
    public void RemoveCable_removes_existing_type()
    {
        var psu = Create();
        var cable = new PsuCable(psu.Id, PsuCableType.Cpu4Plus4Pin, 2, 1);
        psu.AddCable(cable);

        psu.RemoveCable(cable);

        psu.Cables.Should().BeEmpty();
    }

    [Fact]
    public void RemoveCable_rejects_missing_type()
    {
        var psu = Create();
        var cable = new PsuCable(psu.Id, PsuCableType.Molex, 1, 1);

        var act = () => psu.RemoveCable(cable);

        act.Should().Throw<ArgumentException>().WithMessage("*does not exist*");
    }

    private static Psu Create(
        string name = "RM850x",
        int wattage = 850,
        PsuModularity modularity = PsuModularity.FullModular,
        PsuFormFactor formFactor = PsuFormFactor.Atx,
        decimal lengthMm = 160,
        decimal widthMm = 150,
        decimal heightMm = 86) =>
        new(name, ManufacturerId, new PsuSpecs
        {
            Wattage = wattage,
            Modularity = modularity,
            FormFactor = formFactor,
            LengthMm = lengthMm,
            WidthMm = widthMm,
            HeightMm = heightMm
        });
}
