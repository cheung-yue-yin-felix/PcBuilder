using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record GetCpuQuery(string? Name = "", Guid? ManufacturerId = null, Guid? SocketId = null, Guid? SeriesId = null, DdrGeneration? DdrGeneration = null)
    : IRequest<List<CpuDto>>;
