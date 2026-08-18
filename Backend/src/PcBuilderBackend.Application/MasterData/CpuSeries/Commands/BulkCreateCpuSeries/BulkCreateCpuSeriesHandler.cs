using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;

public class BulkCreateCpuSeriesHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<BulkCreateCpuSeriesCommand, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(BulkCreateCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<CpuSeriesDto>();
        foreach (var cpuSeries in request.CpuSeries.Select(dto => new Domain.Entities.CpuSeries(dto.ManufacturerId, dto.SocketId, dto.Name)))
        {
            context.CpuSeries.Add(cpuSeries);
            result.Add(mapper.Map<CpuSeriesDto>(cpuSeries));
        }
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return result;
    }
}
