using MediatR;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Psus.Queries;

public record FilterPsusQuery(PagedRequest<PsuFilter> Request) : IRequest<PagedResult<PsuListItemDto>>;
