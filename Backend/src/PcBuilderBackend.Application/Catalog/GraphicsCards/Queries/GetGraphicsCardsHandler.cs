using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.Common.Extensions;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;

public class GetGraphicsCardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGraphicsCardsQuery, PagedResult<GraphicsCardListItemDto>>
{
    public async Task<PagedResult<GraphicsCardListItemDto>> Handle(
        GetGraphicsCardsQuery request,
        CancellationToken cancellationToken)
    {
        return await context.GraphicsCards
            .AsNoTracking()
            .Where(x => x.IsActive)
            .WhereIf(!string.IsNullOrWhiteSpace(request.Name), x => x.Name.Contains(request.Name!))
            .WhereIf(request.ManufacturerId.HasValue, x => x.ManufacturerId == request.ManufacturerId)
            .WhereIf(request.GpuId.HasValue, x => x.GpuId == request.GpuId)
            .WhereIf(request.PcieGeneration.HasValue, x => x.PcieGeneration == request.PcieGeneration)
            .WhereIf(request.MinVideoMemoryGb.HasValue, x => x.VideoMemoryGb >= request.MinVideoMemoryGb)
            .WhereIf(request.MaxVideoMemoryGb.HasValue, x => x.VideoMemoryGb <= request.MaxVideoMemoryGb)
            .WhereIf(request.MaxLengthMm.HasValue, x => x.LengthMm <= request.MaxLengthMm)
            .WhereIf(request.MaxPowerConsumptionWatts.HasValue, x => x.PowerConsumptionWatts <= request.MaxPowerConsumptionWatts)
            .ToPagedResultAsync<GraphicsCard, GraphicsCardListItemDto>(
                request.PageIndex,
                request.PageSize,
                mapper.ConfigurationProvider,
                cancellationToken);
    }
}
