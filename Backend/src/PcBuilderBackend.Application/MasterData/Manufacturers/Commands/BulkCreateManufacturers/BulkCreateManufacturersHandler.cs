using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkCreateManufacturers;

public class BulkCreateManufacturersHandler(IApplicationDbContext context, IMapper mapper, ICacheService cache)
    : IRequestHandler<BulkCreateManufacturersCommand, List<ManufacturerDto>>
{
    public async Task<List<ManufacturerDto>> Handle(BulkCreateManufacturersCommand request, CancellationToken cancellationToken)
    {
        var result = new List<Manufacturer>();

        foreach (var entity in request.Names.Select(name => new Manufacturer(name)))
        {
            context.Manufacturers.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);

        return result.Select(mapper.Map<ManufacturerDto>).ToList();
    }
}
