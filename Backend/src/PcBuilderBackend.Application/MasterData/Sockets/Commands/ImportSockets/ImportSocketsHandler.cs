using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Sockets.Commands.ImportSockets;

public class ImportSocketsHandler(
    IRepository<Socket> sockets,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache,
    ILogger<ImportSocketsHandler> logger)
    : IRequestHandler<ImportSocketsCommand, List<SocketDto>>
{
    public async Task<List<SocketDto>> Handle(ImportSocketsCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParseSocketImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<Socket>();

        foreach (var row in rows)
        {
            var entity = new Socket(row.ManufacturerId, row.Name);
            sockets.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Sockets.Prefix, cancellationToken);
        EntityLog.Imported(logger, result.Count, EntityLog.Socket);

        return [.. result.Select(mapper.Map<SocketDto>)];
    }
}
