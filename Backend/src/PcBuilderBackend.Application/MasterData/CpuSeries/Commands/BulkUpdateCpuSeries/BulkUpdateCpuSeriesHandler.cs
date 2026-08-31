using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkUpdateCpuSeries;

public class BulkUpdateCpuSeriesHandler(IRepository<Domain.Entities.CpuSeries> cpuSeries, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
    : IRequestHandler<BulkUpdateCpuSeriesCommand, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(BulkUpdateCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var result = new List<CpuSeriesDto>();

        foreach (var cpuSeriesDto in command.CpuSeries)
        {
            var entity = await cpuSeries.GetByIdAsync(cpuSeriesDto.Id, cancellationToken);

            if (entity is null) return result;

            entity.Rename(cpuSeriesDto.Name);
            entity.UpdateManufacturer(cpuSeriesDto.ManufacturerId);
            entity.UpdateSpecs(cpuSeriesDto.SocketId);

            result.Add(mapper.Map<CpuSeriesDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return result;
    }
}
