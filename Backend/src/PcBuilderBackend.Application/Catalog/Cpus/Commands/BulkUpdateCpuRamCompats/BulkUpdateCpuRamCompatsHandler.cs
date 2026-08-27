using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuRamCompats;

public class BulkUpdateCpuRamCompatsHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateCpuRamCompatsHandler> logger)
    : IRequestHandler<BulkUpdateCpuRamCompatsCommand, List<CpuRamCompatDto>?>
{
    public async Task<List<CpuRamCompatDto>?> Handle(
        BulkUpdateCpuRamCompatsCommand request,
        CancellationToken cancellationToken)
    {
        var cpu = await cpus.GetWithChildrenAsync(request.CpuId, cancellationToken);

        if (cpu == null)
        {
            EntityLog.NotFound(logger, EntityLog.Cpu, request.CpuId);
            return null;
        }

        var existingByKey = cpu.RamCompats
            .ToDictionary(x => (x.DdrGeneration, x.RamModuleCount, x.RamRank));

        var touchedKeys = new HashSet<(DdrGeneration, int, RamRank)>();

        foreach (var compat in request.RamCompats)
        {
            var key = (compat.DdrGeneration, compat.RamModuleCount, compat.RamRank);
            touchedKeys.Add(key);

            if (existingByKey.TryGetValue(key, out var existing))
            {
                existing.UpdateSpecs(
                    request.CpuId,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts);
            }
            else
            {
                cpu.AddRamCompat(new CpuRamCompat(
                    request.CpuId,
                    compat.DdrGeneration,
                    compat.RamModuleCount,
                    compat.RamRank,
                    compat.MaxSpeedMts));
            }
        }

        foreach (var existing in existingByKey.Values
                     .Where(x => !touchedKeys.Contains((x.DdrGeneration, x.RamModuleCount, x.RamRank))))
        {
            cpu.RemoveRamCompat(existing);
            cpus.DeleteRamCompat(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.CpuRamCompatsUpdated(logger, cpu.Id);

        return [.. cpu.RamCompats.Where(x => x.IsActive).Select(mapper.Map<CpuRamCompatDto>)];
    }
}
