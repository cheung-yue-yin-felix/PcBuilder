using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkUpdateChipsets;

public class BulkUpdateChipsetsHandler(
    IRepository<Chipset> chipsets, 
    ILogger<BulkUpdateChipsetsHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkUpdateChipsetsCommand, List<ChipsetDto>?>
{
    public async Task<List<ChipsetDto>?> Handle(BulkUpdateChipsetsCommand command, CancellationToken cancellationToken)
    {
        var result = new List<ChipsetDto>();

        foreach (var chipset in command.Chipsets)
        {
            var entity = await chipsets.GetByIdAsync(chipset.Id, cancellationToken);

            if (entity == null) return null;

            entity.Rename(chipset.Name);
            entity.UpdateManufacturer(chipset.ManufacturerId);
            entity.UpdateSpecs(chipset.SocketId);

            result.Add(mapper.Map<ChipsetDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Chipset);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return result;
    }
}
