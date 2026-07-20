using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<UpdateManufacturerCommand, ManufacturerDto?>
{
    public async Task<ManufacturerDto?> Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers.FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return null;
        
        entity.Rename(request.Name);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        
        return mapper.Map<ManufacturerDto>(entity);
    }
}