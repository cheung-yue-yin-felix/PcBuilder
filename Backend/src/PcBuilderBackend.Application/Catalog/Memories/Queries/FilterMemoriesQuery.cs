using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Memories.Queries;

public record FilterMemoriesQuery(PagedRequest<RamFilter> Request) : IRequest<PagedResult<RamDto>>;