using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;

public class UpdateChipsetHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<UpdateChipsetCommand, ChipsetDto?>
{
    public async Task<ChipsetDto?> Handle(UpdateChipsetCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Chipsets.FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive, cancellationToken);
        if (entity == null) return null;
        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(request.SocketId);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<ChipsetDto>(entity);
    }
}