using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IExcelImportService
{
    Task<List<CpuImportRow>> ParseCpuImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<GpuImportRow>> ParseGpuImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<RamImportRow>> ParseRamImportAsync(Stream stream, CancellationToken cancellationToken);
    Task<List<MotherboardImportRow>> ParseMotherboardImportAsync(Stream stream, CancellationToken cancellationToken);
}