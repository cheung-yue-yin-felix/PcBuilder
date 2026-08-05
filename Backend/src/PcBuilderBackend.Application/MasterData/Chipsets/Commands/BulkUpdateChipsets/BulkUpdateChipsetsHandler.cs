using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkUpdateChipsets;

public class BulkUpdateChipsetsHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkUpdateChipsetsCommand, List<ChipsetDto>?>
{
    public async Task<List<ChipsetDto>?> Handle(BulkUpdateChipsetsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<ChipsetDto>();

        foreach (var chipset in request.Chipsets)
        {
            var entity = await context.Chipsets.FirstOrDefaultAsync(x => x.Id == chipset.Id && x.IsActive, cancellationToken);
            
            if (entity == null) return null;
            
            entity.Rename(chipset.Name);
            entity.UpdateManufacturer(chipset.ManufacturerId);
            entity.UpdateSpecs(chipset.SocketId);
            
            result.Add(mapper.Map<ChipsetDto>(entity));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}