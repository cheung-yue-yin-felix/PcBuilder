using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.UpdateCpuSeries;

public class UpdateCpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateCpuSeriesCommand, CpuSeriesDto?>
{
    public async Task<CpuSeriesDto?> Handle(UpdateCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.CpuSeries.FirstOrDefaultAsync(cs => cs.Id == request.CpuSeriesId && cs.IsActive, cancellationToken);
        if (entity is null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(request.SocketId);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return mapper.Map<CpuSeriesDto>(entity);
    }
}
