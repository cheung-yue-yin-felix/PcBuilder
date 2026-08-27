using MediatR;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.ChassisFans.Queries;

public record GetChassisFansQuery(PagedRequest Request) : IRequest<PagedResult<ChassisFanDto>>;
