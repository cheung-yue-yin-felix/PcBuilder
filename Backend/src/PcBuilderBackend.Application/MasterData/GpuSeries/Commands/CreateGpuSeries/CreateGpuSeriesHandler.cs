using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;

public class CreateGpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<CreateGpuSeriesCommand, GpuSeriesDto>
{
    public async Task<GpuSeriesDto> Handle(CreateGpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.GpuSeries(request.ManufacturerId, request.Name);
        context.GpuSeries.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.GpuSeries.Prefix, cancellationToken);
        return mapper.Map<GpuSeriesDto>(entity);
    }
}
