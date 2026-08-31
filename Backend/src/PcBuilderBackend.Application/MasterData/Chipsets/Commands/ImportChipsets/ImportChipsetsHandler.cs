using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.ImportChipsets;

public class ImportChipsetsHandler(
    IRepository<Chipset> chipsets,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportChipsetsHandler> logger)
    : IRequestHandler<ImportChipsetsCommand, List<ChipsetDto>>
{
    public async Task<List<ChipsetDto>> Handle(ImportChipsetsCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParseChipsetImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);
        await ActiveEntityGuard.EnsureSocketsExist(
            lookup, rows.Select(r => r.SocketId), cancellationToken);

        var result = new List<Chipset>();

        foreach (var row in rows)
        {
            var entity = new Chipset(row.Name, row.ManufacturerId, row.SocketId);
            chipsets.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Chipsets.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.Chipset);

        return [.. result.Select(mapper.Map<ChipsetDto>)];
    }
}
