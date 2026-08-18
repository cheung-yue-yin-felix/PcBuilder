using MediatR;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public record GetMotherboardsQuery(PagedRequest Request) : IRequest<PagedResult<MotherboardListItemDto>>;
