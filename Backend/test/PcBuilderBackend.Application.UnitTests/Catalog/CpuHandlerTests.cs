using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using PcBuilderBackend.Application.Catalog.Cpus;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Queries;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.UnitTests.Catalog;

public class CpuHandlerTests : IDisposable
{
    private readonly AppFixture _fx = new();

    public void Dispose() => _fx.Dispose();

    [Fact]
    public async Task Update_and_soft_delete_cpu()
    {
        var created = await new CreateCpuHandler(
            _fx.Cpus, _fx.UnitOfWork, _fx.Mapper, NullLogger<CreateCpuHandler>.Instance)
            .Handle(ValidCpu("7800X3D"), CancellationToken.None);

        var updated = await new UpdateCpuHandler(
            _fx.Cpus, _fx.UnitOfWork, _fx.Mapper, NullLogger<UpdateCpuHandler>.Instance)
            .Handle(new UpdateCpuCommand
            {
                Id = created.Id,
                Name = "9800X3D",
                ManufacturerId = _fx.Manufacturer.Id,
                SocketId = _fx.Socket.Id,
                SeriesId = _fx.CpuSeries.Id,
                MaxMemoryGb = 192,
                ThermalDesignPower = 120,
                PowerConsumptionWatts = 120
            }, CancellationToken.None);

        updated.Should().NotBeNull();
        updated!.Name.Should().Be("9800X3D");
        updated.MaxMemoryGb.Should().Be(192);

        (await new DeleteCpuHandler(
            _fx.Cpus, _fx.UnitOfWork, NullLogger<DeleteCpuHandler>.Instance)
            .Handle(new DeleteCpuCommand(created.Id), CancellationToken.None)).Should().BeTrue();

        (await _fx.Context.Cpus.AnyAsync()).Should().BeFalse();
        (await new DeleteCpuHandler(
            _fx.Cpus, _fx.UnitOfWork, NullLogger<DeleteCpuHandler>.Instance)
            .Handle(new DeleteCpuCommand(created.Id), CancellationToken.None)).Should().BeFalse();
    }

    [Fact]
    public async Task Query_handlers_delegate_to_read_store()
    {
        var store = Substitute.For<ICpuReadStore>();
        var list = PagedResult<CpuListItemDto>.Empty(0, 10);
        var request = new PagedRequest();
        var filter = new PagedRequest<CpuFilter>(new CpuFilter());
        var cpu = new CpuDto { Id = Guid.NewGuid(), Name = "X" };
        var ram = new List<CpuRamCompatDto>();
        var chipsets = new List<CpuSupportChipsetDto>();

        store.ListAsync(request, Arg.Any<CancellationToken>()).Returns(list);
        store.FilterAsync(filter, Arg.Any<CancellationToken>()).Returns(list);
        store.GetByIdAsync(cpu.Id, Arg.Any<CancellationToken>()).Returns(cpu);
        store.ListRamCompatsAsync(cpu.Id, Arg.Any<CancellationToken>()).Returns(ram);
        store.ListSupportChipsetsAsync(cpu.Id, Arg.Any<CancellationToken>()).Returns(chipsets);

        (await new GetCpusHandler(store).Handle(new GetCpusQuery(request), CancellationToken.None))
            .Should().BeSameAs(list);
        (await new FilterCpusHandler(store).Handle(new FilterCpusQuery(filter), CancellationToken.None))
            .Should().BeSameAs(list);
        (await new GetCpuByIdHandler(store).Handle(new GetCpuByIdQuery(cpu.Id), CancellationToken.None))
            .Should().BeSameAs(cpu);
        (await new ListCompatibleMemoriesHandler(store)
            .Handle(new ListCompatibleMemoriesQuery(cpu.Id), CancellationToken.None)).Should().BeSameAs(ram);
        (await new ListCompatibleChipsetsHandler(store)
            .Handle(new ListCompatibleChipsetsQuery(cpu.Id), CancellationToken.None)).Should().BeSameAs(chipsets);
    }

    private CreateCpuCommand ValidCpu(string name) => new()
    {
        Name = name,
        ManufacturerId = _fx.Manufacturer.Id,
        SocketId = _fx.Socket.Id,
        SeriesId = _fx.CpuSeries.Id,
        MaxMemoryGb = 128,
        ThermalDesignPower = 120,
        PowerConsumptionWatts = 120,
        RamCompats = [new CpuRamCompatDto(DdrGeneration.Ddr5, 2, RamRank.DualRank, 6000)],
        SupportChipsets = [new CpuSupportChipsetDto(_fx.Chipset.Id, _fx.Chipset.Name, false)]
    };
}
