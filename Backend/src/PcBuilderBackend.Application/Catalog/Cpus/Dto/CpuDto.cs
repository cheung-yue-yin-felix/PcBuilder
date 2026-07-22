using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Dto;

public record CpuDto(Guid Id, string Name, Guid ManufacturerId, Guid SocketId, DdrGeneration DdrGeneration, string Series, bool IntegratedGraphics, bool IncludedStockCooler, int ThermalDesignPower);