using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;

public class BulkCreateCpuSeriesHandler(
    IRepository<Domain.Entities.CpuSeries> cpuSeries, 
    ILogger<BulkCreateCpuSeriesHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkCreateCpuSeriesCommand, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(BulkCreateCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var result = new List<CpuSeriesDto>();
        foreach (var entity in command.CpuSeries.Select(dto => new Domain.Entities.CpuSeries(dto.ManufacturerId, dto.SocketId, dto.Name)))
        {
            cpuSeries.Add(entity);
            result.Add(mapper.Map<CpuSeriesDto>(entity));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.CpuSeries);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return result;
    }
}
