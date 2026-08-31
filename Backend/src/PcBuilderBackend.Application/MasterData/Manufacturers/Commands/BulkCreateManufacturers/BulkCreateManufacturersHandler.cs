using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.BulkCreateManufacturers;

public class BulkCreateManufacturersHandler(
    IRepository<Manufacturer> manufacturers, 
    ILogger<BulkCreateManufacturersHandler> logger,
    IUnitOfWork unitOfWork, 
    IMapper mapper, 
    ICacheService cache)
    : IRequestHandler<BulkCreateManufacturersCommand, List<ManufacturerDto>>
{
    public async Task<List<ManufacturerDto>> Handle(BulkCreateManufacturersCommand request, CancellationToken cancellationToken)
    {
        var result = new List<Manufacturer>();

        foreach (var entity in request.Names.Select(name => new Manufacturer(name)))
        {
            manufacturers.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Manufacturer);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);

        return [.. result.Select(mapper.Map<ManufacturerDto>)];
    }
}
