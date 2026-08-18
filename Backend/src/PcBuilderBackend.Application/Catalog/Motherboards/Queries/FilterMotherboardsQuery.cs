using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record FilterMotherboardsQuery(PagedRequest<MotherboardFilter> Request) : IRequest<PagedResult<MotherboardListItemDto>>;
