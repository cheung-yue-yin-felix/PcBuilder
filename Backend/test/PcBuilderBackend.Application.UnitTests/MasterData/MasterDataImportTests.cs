using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.ImportChipsets;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.ImportCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.ImportGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.Manufacturers.Commands.ImportManufacturers;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Application.MasterData.Sockets.Commands.ImportSockets;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.UnitTests.MasterData;

public class MasterDataImportTests : IDisposable
{
    private readonly AppFixture _fx = new();
    private readonly IExcelImportService _excel = Substitute.For<IExcelImportService>();

    public void Dispose() => _fx.Dispose();

    [Fact]
    public async Task Import_manufacturers()
    {
        _excel.ParseManufacturerImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns([new ManufacturerImportRow { RowNumber = 2, Name = "Corsair" }]);

        var imported = await new ImportManufacturersHandler(
                new TestRepository<Manufacturer>(_fx.Context),
                _fx.UnitOfWork,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportManufacturersHandler>.Instance)
            .Handle(new ImportManufacturersCommand(Stream.Null), CancellationToken.None);

        imported.Should().ContainSingle(x => x.Name == "Corsair");
        (await _fx.Context.Manufacturers.CountAsync(x => x.Name == "Corsair")).Should().Be(1);
    }

    [Fact]
    public async Task Import_sockets_and_reject_unknown_manufacturer()
    {
        _excel.ParseSocketImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new SocketImportRow
                {
                    RowNumber = 2,
                    Name = "AM5",
                    ManufacturerId = _fx.Manufacturer.Id
                }
            ]);

        var imported = await SocketsHandler()
            .Handle(new ImportSocketsCommand(Stream.Null), CancellationToken.None);

        imported.Should().ContainSingle(x => x.Name == "AM5");

        _excel.ParseSocketImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new SocketImportRow { Name = "LGA1851", ManufacturerId = Guid.NewGuid() }
            ]);

        var act = () => SocketsHandler()
            .Handle(new ImportSocketsCommand(Stream.Null), CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Manufacturer*");
    }

    [Fact]
    public async Task Import_chipsets_cpu_series_gpu_series_and_gpus()
    {
        _excel.ParseChipsetImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new ChipsetImportRow
                {
                    Name = "X870",
                    ManufacturerId = _fx.Manufacturer.Id,
                    SocketId = _fx.Socket.Id
                }
            ]);
        (await new ImportChipsetsHandler(
                new TestRepository<Chipset>(_fx.Context),
                _fx.UnitOfWork,
                _fx.Lookup,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportChipsetsHandler>.Instance)
            .Handle(new ImportChipsetsCommand(Stream.Null), CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "X870");

        _excel.ParseCpuSeriesImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new CpuSeriesImportRow
                {
                    Name = "Ryzen 9000",
                    ManufacturerId = _fx.Manufacturer.Id,
                    SocketId = _fx.Socket.Id
                }
            ]);
        (await new ImportCpuSeriesHandler(
                new TestRepository<CpuSeries>(_fx.Context),
                _fx.UnitOfWork,
                _fx.Lookup,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportCpuSeriesHandler>.Instance)
            .Handle(new ImportCpuSeriesCommand(Stream.Null), CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "Ryzen 9000");

        _excel.ParseGpuSeriesImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new GpuSeriesImportRow
                {
                    Name = "RTX 50",
                    ManufacturerId = _fx.Manufacturer.Id
                }
            ]);
        (await new ImportGpuSeriesHandler(
                new TestRepository<GpuSeries>(_fx.Context),
                _fx.UnitOfWork,
                _fx.Lookup,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportGpuSeriesHandler>.Instance)
            .Handle(new ImportGpuSeriesCommand(Stream.Null), CancellationToken.None))
            .Should().ContainSingle(x => x.Name == "RTX 50");

        _excel.ParseGpuImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new GpuImportRow
                {
                    Name = "RTX 5070",
                    ManufacturerId = _fx.Manufacturer.Id,
                    SeriesId = _fx.GpuSeries.Id
                }
            ]);
        var importedGpus = await new ImportGpusHandler(
                new TestRepository<Gpu>(_fx.Context),
                _fx.UnitOfWork,
                _fx.Lookup,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportGpusHandler>.Instance)
            .Handle(new ImportGpusCommand(Stream.Null), CancellationToken.None);
        importedGpus.Should().ContainSingle(x =>
            x.Name == "RTX 5070"
            && x.GpuSeriesId == _fx.GpuSeries.Id
            && x.ManufacturerId == _fx.Manufacturer.Id);
    }

    [Fact]
    public async Task Import_chipset_rejects_unknown_socket()
    {
        _excel.ParseChipsetImportAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(
            [
                new ChipsetImportRow
                {
                    Name = "Z890",
                    ManufacturerId = _fx.Manufacturer.Id,
                    SocketId = Guid.NewGuid()
                }
            ]);

        var act = () => new ImportChipsetsHandler(
                new TestRepository<Chipset>(_fx.Context),
                _fx.UnitOfWork,
                _fx.Lookup,
                _excel,
                _fx.Mapper,
                _fx.Cache,
                NullLogger<ImportChipsetsHandler>.Instance)
            .Handle(new ImportChipsetsCommand(Stream.Null), CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Socket*");
    }

    private ImportSocketsHandler SocketsHandler() =>
        new(
            new TestRepository<Socket>(_fx.Context),
            _fx.UnitOfWork,
            _fx.Lookup,
            _excel,
            _fx.Mapper,
            _fx.Cache,
            NullLogger<ImportSocketsHandler>.Instance);
}
