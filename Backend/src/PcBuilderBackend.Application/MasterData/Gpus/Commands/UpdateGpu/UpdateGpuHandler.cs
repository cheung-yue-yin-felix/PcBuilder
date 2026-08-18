using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;

public class UpdateGpuHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateGpuCommand, GpuDto?>
{
    public async Task<GpuDto?> Handle(UpdateGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = await context.Gpus.FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        if (gpu is null) return null;

        gpu.Rename(request.Name);
        gpu.UpdateSeries(request.GpuSeriesId);
        gpu.UpdateManufacturer(request.ManufacturerId);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);
        return mapper.Map<GpuDto>(gpu);
    }
}
