using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;

public class UpdateChipsetHandler(
    IRepository<Chipset> chipsets, 
    ILogger<UpdateChipsetHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<UpdateChipsetCommand, ChipsetDto?>
{
    public async Task<ChipsetDto?> Handle(UpdateChipsetCommand command, CancellationToken cancellationToken)
    {
        var entity = await chipsets.GetByIdAsync(command.Id, cancellationToken);
        if (entity == null) return null;
        entity.Rename(command.Name);
        entity.UpdateManufacturer(command.ManufacturerId);
        entity.UpdateSpecs(command.SocketId);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Updated(logger, EntityLog.Chipset, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return mapper.Map<ChipsetDto>(entity);
    }
}
