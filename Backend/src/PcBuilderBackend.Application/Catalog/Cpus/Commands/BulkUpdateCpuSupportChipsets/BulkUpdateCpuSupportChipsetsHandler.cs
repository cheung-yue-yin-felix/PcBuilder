using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuSupportChipsets;

public class BulkUpdateCpuSupportChipsetsHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateCpuSupportChipsetsHandler> logger)
    : IRequestHandler<BulkUpdateCpuSupportChipsetsCommand, List<CpuSupportChipsetDto>?>
{
    public async Task<List<CpuSupportChipsetDto>?> Handle(
        BulkUpdateCpuSupportChipsetsCommand request,
        CancellationToken cancellationToken)
    {
        var cpu = await cpus.GetWithChildrenAsync(request.CpuId, cancellationToken);

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
            cpus.DeleteSupportedChipset(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.CpuSupportChipsetsUpdated(logger, cpu.Id);

        return [.. cpu.SupportedChipsets.Where(x => x.IsActive).Select(mapper.Map<CpuSupportChipsetDto>)];
    }
}
