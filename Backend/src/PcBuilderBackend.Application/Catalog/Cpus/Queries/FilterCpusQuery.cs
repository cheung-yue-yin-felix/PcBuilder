using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record FilterCpusQuery(PagedRequest<CpuFilter> Request) : IRequest<PagedResult<CpuListItemDto>>;