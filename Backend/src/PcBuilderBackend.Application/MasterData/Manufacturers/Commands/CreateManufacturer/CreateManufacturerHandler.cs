using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<CreateManufacturerCommand, ManufacturerDto>
{
    public async Task<ManufacturerDto> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Manufacturer(request.Name);
        context.Manufacturers.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);
        return mapper.Map<ManufacturerDto>(entity);
    }
}
