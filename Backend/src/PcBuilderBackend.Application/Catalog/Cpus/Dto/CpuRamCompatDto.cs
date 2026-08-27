using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuRamCompatDto(DdrGeneration DdrGeneration, int RamModuleCount, RamRank RamRank, int MaxSpeedMts);