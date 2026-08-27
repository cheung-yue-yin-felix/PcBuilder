using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpus;

public class BulkUpdateCpusHandler(
    ICpuRepository cpus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateCpusHandler> logger)
    : IRequestHandler<BulkUpdateCpusCommand, List<CpuDto>?>
{
    public async Task<List<CpuDto>?> Handle(BulkUpdateCpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<CpuDto>();

        foreach (var command in request.Cpus)
        {
            var entity = await cpus.GetWithChildrenAsync(command.Id, cancellationToken);

            if (entity == null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.Cpu, command.Id);
                return null;
            }

            entity.Rename(command.Name);
            entity.UpdateManufacturer(command.ManufacturerId);
            entity.UpdateSpecs(
                command.SocketId,
                command.SeriesId,
                command.MaxMemoryGb,
                command.IntegratedGraphics,
                command.IncludedStockCooler,
                command.ThermalDesignPower,
                command.PowerConsumptionWatts);

            result.Add(mapper.Map<CpuDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Cpu);

        return result;
    }
}
