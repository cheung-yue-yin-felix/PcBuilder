using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuFilter(
    Guid? ManufacturerId = null,
    string? Name = "",
    Guid? SocketId = null,
    Guid? SeriesId = null,
    DdrGeneration? DdrGeneration = null,
    Guid? MotherboardId = null,
    RangeFilter? PowerConsumptionWatts = null,
    RangeFilter? ThermalDesignPower = null
);