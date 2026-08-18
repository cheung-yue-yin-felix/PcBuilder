using MediatR;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.Chassis.Queries;

public record FilterChassisQuery(PagedRequest<ChassisFilter> Request) : IRequest<PagedResult<ChassisListItemDto>>;