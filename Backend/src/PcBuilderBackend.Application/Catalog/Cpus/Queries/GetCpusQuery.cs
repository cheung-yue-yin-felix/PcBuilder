using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record GetCpusQuery(PagedRequest Request) : IRequest<PagedResult<CpuListItemDto>>;
