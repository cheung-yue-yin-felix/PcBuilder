using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkCreatePsus;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkDeletePsus;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsuCables;
using PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsus;
using PcBuilderBackend.Application.Catalog.Psus.Commands.CreatePsu;
using PcBuilderBackend.Application.Catalog.Psus.Commands.DeletePsu;
using PcBuilderBackend.Application.Catalog.Psus.Commands.ImportPsus;
using PcBuilderBackend.Application.Catalog.Psus.Commands.UpdatePsu;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.UnitTests.Catalog.Psus;

public class PsuCommandHandlerTests : IDisposable
{
    private readonly PsuCatalogFixture _fixture = new();

    public void Dispose() => _fixture.Dispose();

    [Fact]
    public async Task Create_persists_psu_and_cables()
    {
        var handler = new CreatePsuHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Mapper, NullLogger<CreatePsuHandler>.Instance);

        var result = await handler.Handle(_fixture.ValidCreate(), CancellationToken.None);

        result.Name.Should().Be("RM850x");
        result.Wattage.Should().Be(850);
        result.Cables.Should().ContainSingle(c => c.Type == PsuCableType.Motherboard24Pin);
        (await _fixture.Context.Psus.CountAsync()).Should().Be(1);
        (await _fixture.Context.PsuCables.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Update_changes_specs_and_returns_null_when_missing()
    {
        var psu = _fixture.SeedPsu();
        var handler = new UpdatePsuHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Mapper, NullLogger<UpdatePsuHandler>.Instance);

        var updated = await handler.Handle(new UpdatePsuCommand
        {
            Id = psu.Id,
            Name = "SF750",
            ManufacturerId = _fixture.Manufacturer.Id,
            Wattage = 750,
            Modularity = PsuModularity.SemiModular,
            FormFactor = PsuFormFactor.Sfx,
            LengthMm = 100,
            WidthMm = 125,
            HeightMm = 63.5m
        }, CancellationToken.None);

        updated.Should().NotBeNull();
        updated!.Name.Should().Be("SF750");
        updated.Wattage.Should().Be(750);
        updated.FormFactor.Should().Be(PsuFormFactor.Sfx);

        var missing = await handler.Handle(new UpdatePsuCommand
        {
            Id = Guid.NewGuid(),
            Name = "Missing",
            ManufacturerId = _fixture.Manufacturer.Id,
            Wattage = 850,
            Modularity = PsuModularity.FullModular,
            FormFactor = PsuFormFactor.Atx,
            LengthMm = 160,
            WidthMm = 150,
            HeightMm = 86
        }, CancellationToken.None);

        missing.Should().BeNull();
    }

    [Fact]
    public async Task Delete_soft_deletes_and_returns_false_when_missing()
    {
        var psu = _fixture.SeedPsu();
        var handler = new DeletePsuHandler(
            _fixture.Psus, _fixture.UnitOfWork, NullLogger<DeletePsuHandler>.Instance);

        (await handler.Handle(new DeletePsuCommand(psu.Id), CancellationToken.None)).Should().BeTrue();
        (await _fixture.Context.Psus.FirstOrDefaultAsync(x => x.Id == psu.Id)).Should().BeNull();
        (await handler.Handle(new DeletePsuCommand(Guid.NewGuid()), CancellationToken.None)).Should().BeFalse();
    }

    [Fact]
    public async Task Bulk_create_inserts_all_items()
    {
        var handler = new BulkCreatePsusHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Mapper, NullLogger<BulkCreatePsusHandler>.Instance);

        var result = await handler.Handle(new BulkCreatePsusCommand(
        [
            new CreatePsuItem
            {
                Name = "RM850x",
                ManufacturerId = _fixture.Manufacturer.Id,
                Wattage = 850,
                Modularity = PsuModularity.FullModular,
                FormFactor = PsuFormFactor.Atx,
                LengthMm = 160,
                WidthMm = 150,
                HeightMm = 86
            },
            new CreatePsuItem
            {
                Name = "SF750",
                ManufacturerId = _fixture.Manufacturer.Id,
                Wattage = 750,
                Modularity = PsuModularity.FullModular,
                FormFactor = PsuFormFactor.Sfx,
                LengthMm = 100,
                WidthMm = 125,
                HeightMm = 63.5m
            }
        ]), CancellationToken.None);

        result.Should().HaveCount(2);
        (await _fixture.Context.Psus.CountAsync()).Should().Be(2);
    }

    [Fact]
    public async Task Bulk_update_aborts_when_any_item_is_missing()
    {
        var psu = _fixture.SeedPsu();
        var handler = new BulkUpdatePsusHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Mapper, NullLogger<BulkUpdatePsusHandler>.Instance);

        var result = await handler.Handle(new BulkUpdatePsusCommand(
        [
            new UpdatePsuCommand
            {
                Id = psu.Id,
                Name = "RM850x",
                ManufacturerId = _fixture.Manufacturer.Id,
                Wattage = 850,
                Modularity = PsuModularity.FullModular,
                FormFactor = PsuFormFactor.Atx,
                LengthMm = 160,
                WidthMm = 150,
                HeightMm = 86
            },
            new UpdatePsuCommand
            {
                Id = Guid.NewGuid(),
                Name = "Missing",
                ManufacturerId = _fixture.Manufacturer.Id,
                Wattage = 850,
                Modularity = PsuModularity.FullModular,
                FormFactor = PsuFormFactor.Atx,
                LengthMm = 160,
                WidthMm = 150,
                HeightMm = 86
            }
        ]), CancellationToken.None);

        result.Should().BeNull();
    }

    [Fact]
    public async Task Bulk_delete_requires_all_ids_to_exist()
    {
        var psu = _fixture.SeedPsu();
        var handler = new BulkDeletePsusHandler(
            _fixture.Psus, _fixture.UnitOfWork, NullLogger<BulkDeletePsusHandler>.Instance);

        (await handler.Handle(new BulkDeletePsusCommand([psu.Id, Guid.NewGuid()]), CancellationToken.None))
            .Should().BeFalse();
        (await _fixture.Context.Psus.CountAsync()).Should().Be(1);

        (await handler.Handle(new BulkDeletePsusCommand([psu.Id]), CancellationToken.None)).Should().BeTrue();
        (await _fixture.Context.Psus.CountAsync()).Should().Be(0);
    }

    [Fact]
    public async Task Bulk_update_cables_adds_updates_and_removes()
    {
        var psu = _fixture.SeedPsu();
        psu.AddCable(new PsuCable(psu.Id, PsuCableType.Sata, 2, 4));
        psu.AddCable(new PsuCable(psu.Id, PsuCableType.Molex, 1, 1));
        await _fixture.Context.SaveChangesAsync();

        var handler = new BulkUpdatePsuCablesHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Mapper, NullLogger<BulkUpdatePsuCablesHandler>.Instance);

        var result = await handler.Handle(new BulkUpdatePsuCablesCommand(psu.Id,
        [
            new PsuCableDto { Type = PsuCableType.Sata, CablesCount = 4, ConnectorsCount = 4 },
            new PsuCableDto { Type = PsuCableType.Motherboard24Pin, CablesCount = 1, ConnectorsCount = 1 }
        ]), CancellationToken.None);

        result.Should().HaveCount(2);
        result!.Should().Contain(c => c.Type == PsuCableType.Sata && c.CablesCount == 4);
        result.Should().Contain(c => c.Type == PsuCableType.Motherboard24Pin);
        result.Should().NotContain(c => c.Type == PsuCableType.Molex);

        (await handler.Handle(new BulkUpdatePsuCablesCommand(Guid.NewGuid(), []), CancellationToken.None))
            .Should().BeNull();
    }

    [Fact]
    public async Task Import_creates_rows_and_rejects_unknown_manufacturers()
    {
        var excel = Substitute.For<IExcelImportService>();
        excel.ParsePsuImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new PsuImportRow
                {
                    RowNumber = 2,
                    Name = "RM850x",
                    ManufacturerId = _fixture.Manufacturer.Id,
                    Wattage = 850,
                    Modularity = PsuModularity.FullModular,
                    FormFactor = PsuFormFactor.Atx,
                    LengthMm = 160,
                    WidthMm = 150,
                    HeightMm = 86,
                    Cables =
                    [
                        new PsuCableImportRow
                        {
                            ParentRowNumber = 2,
                            Type = PsuCableType.Motherboard24Pin,
                            CablesCount = 1,
                            ConnectorsCount = 1
                        }
                    ]
                }
            ]);

        var handler = new ImportPsusHandler(
            _fixture.Psus, _fixture.UnitOfWork, _fixture.Lookup, excel, _fixture.Mapper,
            NullLogger<ImportPsusHandler>.Instance);

        var imported = await handler.Handle(new ImportPsusCommand(Stream.Null), CancellationToken.None);

        imported.Should().ContainSingle(x => x.Name == "RM850x");
        imported[0].Cables.Should().ContainSingle();

        excel.ParsePsuImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new PsuImportRow
                {
                    Name = "Bad",
                    ManufacturerId = Guid.NewGuid(),
                    Wattage = 850,
                    Modularity = PsuModularity.FullModular,
                    FormFactor = PsuFormFactor.Atx,
                    LengthMm = 160,
                    WidthMm = 150,
                    HeightMm = 86
                }
            ]);

        var act = () => handler.Handle(new ImportPsusCommand(Stream.Null), CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Manufacturer*");
    }
}
