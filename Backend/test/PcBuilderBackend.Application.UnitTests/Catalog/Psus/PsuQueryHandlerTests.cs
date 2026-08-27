using FluentAssertions;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Catalog.Psus.Queries;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.UnitTests.Catalog.Psus;

public class PsuQueryHandlerTests : IDisposable
{
    private readonly PsuCatalogFixture _fixture = new();

    public void Dispose() => _fixture.Dispose();

    [Fact]
    public async Task Get_by_id_returns_dto_with_cables_or_null()
    {
        var psu = _fixture.SeedPsu();
        psu.AddCable(new PsuCable(psu.Id, PsuCableType.Motherboard24Pin, 1, 1));
        await _fixture.Context.SaveChangesAsync();
        var handler = new GetPsuByIdHandler(_fixture.Context, _fixture.Mapper);

        var found = await handler.Handle(new GetPsuByIdQuery(psu.Id), CancellationToken.None);
        var missing = await handler.Handle(new GetPsuByIdQuery(Guid.NewGuid()), CancellationToken.None);

        found.Should().NotBeNull();
        found!.Name.Should().Be("RM850x");
        found.ManufacturerName.Should().Be("Corsair");
        found.Cables.Should().ContainSingle();
        missing.Should().BeNull();
    }

    [Fact]
    public async Task Get_list_pages_results()
    {
        _fixture.SeedPsu("RM850x");
        _fixture.SeedPsu("SF750");
        var handler = new GetPsusHandler(_fixture.Context, _fixture.Mapper);

        var page = await handler.Handle(
            new GetPsusQuery(new PagedRequest(PageIndex: 0, PageSize: 1, SortBy: "name")),
            CancellationToken.None);

        page.TotalCount.Should().Be(2);
        page.Items.Should().HaveCount(1);
        page.Items[0].Name.Should().Be("RM850x");
    }

    [Fact]
    public async Task Get_cables_returns_empty_when_psu_is_missing()
    {
        var psu = _fixture.SeedPsu();
        psu.AddCable(new PsuCable(psu.Id, PsuCableType.Sata, 2, 4));
        await _fixture.Context.SaveChangesAsync();
        var handler = new GetPsuCablesByPsuIdHandler(_fixture.Context, _fixture.Mapper);

        var cables = await handler.Handle(new GetPsuCablesByPsuIdQuery(psu.Id), CancellationToken.None);
        var missing = await handler.Handle(new GetPsuCablesByPsuIdQuery(Guid.NewGuid()), CancellationToken.None);

        cables.Should().ContainSingle(c => c.Type == PsuCableType.Sata);
        missing.Should().BeEmpty();
    }

    [Fact]
    public async Task Filter_applies_wattage_and_name()
    {
        _fixture.SeedPsu("RM850x", wattage: 850);
        _fixture.SeedPsu("SF750", wattage: 750);
        var handler = new FilterPsusHandler(_fixture.Context, _fixture.Mapper);

        var page = await handler.Handle(new FilterPsusQuery(new PagedRequest<PsuFilter>(new PsuFilter
        {
            Name = "RM",
            Wattage = new RangeFilter { Min = 800, Max = 900 }
        })), CancellationToken.None);

        page.Items.Should().ContainSingle(x => x.Name == "RM850x");
    }

    [Fact]
    public async Task Filter_returns_empty_when_chassis_is_missing()
    {
        _fixture.SeedPsu();
        var handler = new FilterPsusHandler(_fixture.Context, _fixture.Mapper);

        var page = await handler.Handle(new FilterPsusQuery(new PagedRequest<PsuFilter>(new PsuFilter
        {
            ChassisId = Guid.NewGuid()
        })), CancellationToken.None);

        page.TotalCount.Should().Be(0);
        page.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Filter_keeps_psus_compatible_with_chassis()
    {
        _fixture.SeedPsu("ATX-160", formFactor: PsuFormFactor.Atx, lengthMm: 160);
        _fixture.SeedPsu("ATX-200", formFactor: PsuFormFactor.Atx, lengthMm: 200);
        _fixture.SeedPsu("SFX-100", formFactor: PsuFormFactor.Sfx, lengthMm: 100);

        var chassis = new Chassis(
            "4000D",
            _fixture.Manufacturer.Id,
            450, 230, 460,
            305, 244,
            170, 370, 180);
        chassis.AddPsuFormFactor(new ChassisPsuFormFactor(chassis.Id, PsuFormFactor.Atx));
        _fixture.Context.Chassis.Add(chassis);
        await _fixture.Context.SaveChangesAsync();

        var handler = new FilterPsusHandler(_fixture.Context, _fixture.Mapper);
        var page = await handler.Handle(new FilterPsusQuery(new PagedRequest<PsuFilter>(new PsuFilter
        {
            ChassisId = chassis.Id
        })), CancellationToken.None);

        page.Items.Should().ContainSingle(x => x.Name == "ATX-160");
    }
}
