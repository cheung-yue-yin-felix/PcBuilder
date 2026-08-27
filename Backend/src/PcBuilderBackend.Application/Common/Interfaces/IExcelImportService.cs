using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Catalog.StorageDrives.Dto;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IExcelImportService
{
    Task<List<CpuImportRow>> ParseCpuImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<GpuImportRow>> ParseGpuImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<RamImportRow>> ParseRamImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<MotherboardImportRow>> ParseMotherboardImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<ChassisImportRow>> ParseChassisImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<CpuCoolerImportRow>> ParseCpuCoolerImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<GraphicsCardImportRow>> ParseGraphicsCardImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<ChassisFanImportRow>> ParseChassisFanImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<PsuImportRow>> ParsePsuImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<StorageDriveImportRow>> ParseStorageDriveImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<WiredNetworkAdapterImportRow>> ParseWiredNetworkAdapterImportAsync(
        Stream stream, CancellationToken cancellationToken);
    Task<List<WirelessNetworkAdapterImportRow>> ParseWirelessNetworkAdapterImportAsync(
        Stream stream, CancellationToken cancellationToken);
}