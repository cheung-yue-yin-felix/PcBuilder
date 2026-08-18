using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.UpdateManufacturer;

public class UpdateManufacturerHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<UpdateManufacturerCommand, ManufacturerDto?>
{
    public async Task<ManufacturerDto?> Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Manufacturers.FirstOrDefaultAsync(m => m.Id == request.Id && m.IsActive, cancellationToken);
        if (entity == null) return null;

        entity.Rename(request.Name);
        entity.UpdatedAtUtc = DateTime.UtcNow;
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);

        return mapper.Map<ManufacturerDto>(entity);
    }
}
