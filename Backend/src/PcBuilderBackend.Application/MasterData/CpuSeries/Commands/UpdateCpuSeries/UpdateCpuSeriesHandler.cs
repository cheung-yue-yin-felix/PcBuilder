using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.UpdateCpuSeries;

public class UpdateCpuSeriesHandler(IRepository<Domain.Entities.CpuSeries> cpuSeries, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateCpuSeriesCommand, CpuSeriesDto?>
{
    public async Task<CpuSeriesDto?> Handle(UpdateCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = await cpuSeries.GetByIdAsync(command.CpuSeriesId, cancellationToken);
        if (entity is null) return null;

        entity.Rename(command.Name);
        entity.UpdateManufacturer(command.ManufacturerId);
        entity.UpdateSpecs(command.SocketId);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return mapper.Map<CpuSeriesDto>(entity);
    }
}
