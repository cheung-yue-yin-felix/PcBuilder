using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Commands.BulkUpdateCpuCoolerSockets;

public class BulkUpdateCpuCoolerSocketsHandler(
    ICpuCoolerRepository cpuCoolers,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateCpuCoolerSocketsHandler> logger)
    : IRequestHandler<BulkUpdateCpuCoolerSocketsCommand, List<CpuCoolerSocketDto>?>
{
    public async Task<List<CpuCoolerSocketDto>?> Handle(
        BulkUpdateCpuCoolerSocketsCommand request,
        CancellationToken cancellationToken)
    {
        var cooler = await cpuCoolers.GetWithChildrenAsync(request.CpuCoolerId, cancellationToken);

        if (cooler is null)
        {
            EntityLog.NotFound(logger, EntityLog.CpuCooler, request.CpuCoolerId);
            return null;
        }

        SyncSockets(unitOfWork, cpuCoolers, cooler, request.Sockets);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.CpuCoolerSocketsUpdated(logger, cooler.Id);

        return [.. cooler.CpuCoolerSockets.Select(mapper.Map<CpuCoolerSocketDto>)];
    }

    internal static void SyncSockets(
        IUnitOfWork context,
        ICpuCoolerRepository cpuCoolers,
        CpuCooler cooler,
        IEnumerable<CpuCoolerSocketDto> sockets)
    {
        var requested = sockets.Select(s => s.SocketId).Distinct().ToHashSet();
        var existingBySocketId = cooler.CpuCoolerSockets.ToDictionary(x => x.SocketId);

        foreach (var socketId in requested.Where(socketId => !existingBySocketId.ContainsKey(socketId)))
        {
            cooler.AddCpuCoolerSocket(new CpuCoolerSocket(cooler.Id, socketId));
        }

        foreach (var existing in existingBySocketId.Values.Where(x => !requested.Contains(x.SocketId)))
        {
            cooler.RemoveCpuCoolerSocket(existing);
            cpuCoolers.DeleteCpuCoolerSocket(existing);
        }
    }
}
