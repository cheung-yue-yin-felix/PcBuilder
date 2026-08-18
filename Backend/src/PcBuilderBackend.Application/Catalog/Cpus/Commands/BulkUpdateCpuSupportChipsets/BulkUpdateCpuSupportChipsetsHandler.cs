using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuSupportChipsets;

public class BulkUpdateCpuSupportChipsetsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkUpdateCpuSupportChipsetsHandler> logger)
    : IRequestHandler<BulkUpdateCpuSupportChipsetsCommand, List<CpuSupportChipsetDto>?>
{
    public async Task<List<CpuSupportChipsetDto>?> Handle(
        BulkUpdateCpuSupportChipsetsCommand request,
        CancellationToken cancellationToken)
    {
        var cpu = await context.Cpus
            .Include(x => x.SupportedChipsets)
            .FirstOrDefaultAsync(x => x.Id == request.CpuId, cancellationToken);

        if (cpu == null)
        {
            EntityLog.NotFound(logger, EntityLog.Cpu, request.CpuId);
            return null;
        }

        var existingByChipsetId = cpu.SupportedChipsets.ToDictionary(x => x.ChipsetId);
        var touchedChipsetIds = new HashSet<Guid>();

        foreach (var support in request.SupportChipsets)
        {
            touchedChipsetIds.Add(support.ChipsetId);

            if (existingByChipsetId.TryGetValue(support.ChipsetId, out var existing))
            {
                existing.UpdateSpecs(request.CpuId, support.ChipsetId, support.RequiresBiosUpdate);
            }
            else
            {
                cpu.AddSupportedChipset(new CpuSupportChipset(
                    request.CpuId,
                    support.ChipsetId,
                    support.RequiresBiosUpdate));
            }
        }

        foreach (var existing in existingByChipsetId.Values
                     .Where(x => !touchedChipsetIds.Contains(x.ChipsetId)))
        {
            cpu.RemoveSupportedChipset(existing);
            context.CpuSupportChipsets.Remove(existing);
        }

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.CpuSupportChipsetsUpdated(logger, cpu.Id);

        return await context.CpuSupportChipsets
            .AsNoTracking()
            .Where(x => x.CpuId == request.CpuId)
            .ProjectTo<CpuSupportChipsetDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
