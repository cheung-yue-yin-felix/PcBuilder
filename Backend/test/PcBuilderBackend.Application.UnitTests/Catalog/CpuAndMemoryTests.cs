using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Validators;
using PcBuilderBackend.Application.Catalog.Memories;
using PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;
using PcBuilderBackend.Application.Catalog.Memories.Validators;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.CreateStorageDrive;
using PcBuilderBackend.Application.Catalog.StorageDrives.Commands.DeleteStorageDrive;
using PcBuilderBackend.Application.Catalog.StorageDrives.Validators;
using PcBuilderBackend.Application.UnitTests.Support;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.UnitTests.Catalog;

public class CpuAndMemoryTests : IDisposable
{
    private readonly AppFixture _fx = new();

    public void Dispose() => _fx.Dispose();

    [Fact]
    public async Task Create_cpu_validator_requires_related_master_data_and_child_rows()
    {
        var validator = new CreateCpuCommandValidator(_fx.Lookup);
        var invalid = new CreateCpuCommand { Name = "CPU" };
        (await validator.ValidateAsync(invalid)).IsValid.Should().BeFalse();

        var valid = ValidCpu();
        (await validator.ValidateAsync(valid)).IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Create_cpu_persists_ram_compats_and_chipset_support()
    {
        var handler = new CreateCpuHandler(
            _fx.Cpus, _fx.UnitOfWork, _fx.Mapper, NullLogger<CreateCpuHandler>.Instance);

        var result = await handler.Handle(ValidCpu(), CancellationToken.None);

        result.Name.Should().Be("7800X3D");
        result.RamCompats.Should().ContainSingle();
        result.SupportChipsets.Should().ContainSingle(x => x.ChipsetId == _fx.Chipset.Id);
        (await _fx.Context.Cpus.CountAsync()).Should().Be(1);
        (await _fx.Context.CpuRamCompats.CountAsync()).Should().Be(1);
        (await _fx.Context.CpuSupportChipsets.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Create_memory_validator_and_handler()
    {
        var validator = new CreateMemoryCommandValidator(_fx.Lookup);
        var command = new CreateMemoryCommand
        {
            Name = "Vengeance",
            ManufacturerId = _fx.Manufacturer.Id,
            Color = "Black",
            DdrGeneration = DdrGeneration.Ddr5,
            RamFormFactor = RamFormFactor.UDimm,
            RamRank = RamRank.DualRank,
            MemorySizePerStickGb = 16,
            TotalMemorySizeGb = 32,
            ModulesCount = 2,
            MaxMemorySpeedMts = 6000,
            HeightMm = 40
        };
        (await validator.ValidateAsync(command)).IsValid.Should().BeTrue();
        (await validator.ValidateAsync(command with { TotalMemorySizeGb = 8 })).IsValid.Should().BeFalse();

        var rams = Substitute.For<IRamRepository>();
        rams.When(x => x.Add(Arg.Any<Ram>())).Do(ci => _fx.Context.Rams.Add(ci.Arg<Ram>()));
        var dto = await new CreateMemoryHandler(rams, _fx.UnitOfWork, _fx.Mapper)
            .Handle(command, CancellationToken.None);
        dto.Name.Should().Be("Vengeance");
        (await _fx.Context.Rams.CountAsync()).Should().Be(1);
    }

    [Fact]
    public async Task Storage_drive_create_and_soft_delete()
    {
        var validator = new CreateStorageDriveCommandValidator(_fx.Lookup);
        var ssd = new CreateStorageDriveCommand
        {
            Name = "990 PRO",
            ManufacturerId = _fx.Manufacturer.Id,
            Media = StorageMedia.Ssd,
            Interface = StorageInterface.Nvme,
            FormFactor = StorageFormFactor.M22280,
            CapacityGb = 2000,
            PcieGeneration = PcieGeneration.Gen4
        };
        (await validator.ValidateAsync(ssd)).IsValid.Should().BeTrue();
        (await validator.ValidateAsync(ssd with { Media = StorageMedia.Hdd })).IsValid.Should().BeFalse();

        var created = await new CreateStorageDriveHandler(
                new TestStorageDriveRepository(_fx.Context),
                _fx.UnitOfWork,
                _fx.Mapper,
                NullLogger<CreateStorageDriveHandler>.Instance)
            .Handle(ssd, CancellationToken.None);
        created.IsM2.Should().BeTrue();

        (await new DeleteStorageDriveHandler(
                new TestStorageDriveRepository(_fx.Context),
                _fx.UnitOfWork,
                NullLogger<DeleteStorageDriveHandler>.Instance)
            .Handle(new DeleteStorageDriveCommand(created.Id), CancellationToken.None)).Should().BeTrue();
        (await _fx.Context.StorageDrives.AnyAsync()).Should().BeFalse();
    }

    private CreateCpuCommand ValidCpu() => new()
    {
        Name = "7800X3D",
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
