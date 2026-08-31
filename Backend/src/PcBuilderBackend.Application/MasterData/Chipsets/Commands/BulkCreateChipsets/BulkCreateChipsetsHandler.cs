using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;

public class BulkCreateChipsetsHandler(
    IRepository<Chipset> chipsets, 
    ILogger<BulkCreateChipsetsHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkCreateChipsetsCommand, List<ChipsetDto>>
{
    public async Task<List<ChipsetDto>> Handle(BulkCreateChipsetsCommand command, CancellationToken cancellationToken)
    {
        var result = new List<ChipsetDto>();

        foreach (var entity in command.Chipsets.Select(dto => new Chipset(dto.Name, dto.ManufacturerId, dto.SocketId)))
        {
            chipsets.Add(entity);
            result.Add(mapper.Map<ChipsetDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Chipset);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return result;
    }
}
