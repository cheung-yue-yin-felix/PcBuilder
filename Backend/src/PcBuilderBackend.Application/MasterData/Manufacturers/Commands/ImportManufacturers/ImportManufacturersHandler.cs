using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Commands.ImportManufacturers;

public class ImportManufacturersHandler(
    IRepository<Manufacturer> manufacturers,
    IUnitOfWork unitOfWork,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportManufacturersHandler> logger)
    : IRequestHandler<ImportManufacturersCommand, List<ManufacturerDto>>
{
    public async Task<List<ManufacturerDto>> Handle(
        ImportManufacturersCommand request,
        CancellationToken cancellationToken)
    {
        var rows = await excel.ParseManufacturerImportAsync(request.Stream, cancellationToken);
        var result = new List<Manufacturer>();

        foreach (var row in rows)
        {
            var entity = new Manufacturer(row.Name);
            manufacturers.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Manufacturers.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.Manufacturer);

        return [.. result.Select(mapper.Map<ManufacturerDto>)];
    }
}
