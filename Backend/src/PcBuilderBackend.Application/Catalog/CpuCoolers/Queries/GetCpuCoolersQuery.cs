using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public record GetCpuCoolersQuery(PagedRequest Request) : IRequest<PagedResult<CpuCoolerListItemDto>>;
