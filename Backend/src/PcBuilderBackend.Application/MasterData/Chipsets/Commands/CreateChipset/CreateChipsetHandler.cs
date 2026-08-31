using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

public class CreateChipsetHandler(
    IRepository<Chipset> chipsets, 
    ILogger<CreateChipsetHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<CreateChipsetCommand, ChipsetDto>
{
    public async Task<ChipsetDto> Handle(CreateChipsetCommand command, CancellationToken cancellationToken)
    {
        var entity = new Chipset(command.Name, command.ManufacturerId, command.SocketId);
        chipsets.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.Created(logger, EntityLog.Chipset, entity.Id);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        return mapper.Map<ChipsetDto>(entity);
    }
}
