using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record GetCpusQuery(int PageIndex = 0, int PageSize = 10, string? Name = "", Guid? ManufacturerId = null, Guid? SocketId = null, Guid? SeriesId = null, DdrGeneration? DdrGeneration = null)
    : IRequest<PagedResult<CpuListItemDto>>;
