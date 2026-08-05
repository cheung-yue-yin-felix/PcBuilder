using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkUpdateManufacturers;

public class BulkUpdateManufacturersHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkUpdateManufacturersCommand, List<ManufacturerDto>?>
{
    public async Task<List<ManufacturerDto>?> Handle(BulkUpdateManufacturersCommand request, CancellationToken cancellationToken)
    {
        var result = new List<ManufacturerDto>();
        
        foreach (var manufacturer in request.Manufacturers)
        {
            var entity = await context.Manufacturers.FirstOrDefaultAsync(m => m.Id == manufacturer.Id && m.IsActive, cancellationToken);
            
            if (entity == null) return null;
            
            entity.Rename(manufacturer.Name);
            result.Add(mapper.Map<ManufacturerDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}