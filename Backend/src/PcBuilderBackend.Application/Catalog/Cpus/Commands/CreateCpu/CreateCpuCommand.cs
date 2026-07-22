using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;

public record CreateCpuCommand(string Name, Guid ManufacturerId, Guid SocketId, Guid SeriesId, DdrGeneration DdrGeneration, int MaxMemoryGb, bool IntegratedGraphics, bool IncludedStockCooler, int ThermalDesignPower): IRequest<CpuDto>;