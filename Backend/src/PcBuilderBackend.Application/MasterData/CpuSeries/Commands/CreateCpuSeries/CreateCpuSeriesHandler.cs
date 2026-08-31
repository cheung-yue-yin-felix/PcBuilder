using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

public class CreateCpuSeriesHandler(IRepository<Domain.Entities.CpuSeries> cpuSeries, IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
    : IRequestHandler<CreateCpuSeriesCommand, CpuSeriesDto>
{
    public async Task<CpuSeriesDto> Handle(CreateCpuSeriesCommand command, CancellationToken cancellationToken)
    {
        var entity = new Domain.Entities.CpuSeries(command.ManufacturerId, command.SocketId, command.Name);
        cpuSeries.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.CpuSeries.Prefix, cancellationToken);
        return mapper.Map<CpuSeriesDto>(entity);
    }
}
