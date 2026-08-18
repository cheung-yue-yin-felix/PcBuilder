using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;

public class UpdateGpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateGpuSeriesCommand, GpuSeriesDto?>
{
    public async Task<GpuSeriesDto?> Handle(UpdateGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.GpuSeries.FirstOrDefaultAsync(g => g.Id == request.Id && g.IsActive, cancellationToken);
        if (entity == null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return mapper.Map<GpuSeriesDto>(entity);
    }
}
