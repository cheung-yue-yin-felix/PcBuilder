using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsuCables;

public class BulkUpdatePsuCablesHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdatePsuCablesHandler> logger)
    : IRequestHandler<BulkUpdatePsuCablesCommand, List<PsuCableDto>?>
{
    public async Task<List<PsuCableDto>?> Handle(
        BulkUpdatePsuCablesCommand request,
        CancellationToken cancellationToken)
    {
        var psu = await psus.GetWithChildrenAsync(request.PsuId, cancellationToken);

        if (psu is null)
        {
            EntityLog.NotFound(logger, EntityLog.Psu, request.PsuId);
            return null;
        }

        var existingByKey = psu.Cables.ToDictionary(x => x.Type);
        var touchedKeys = new HashSet<PsuCableType>();

        foreach (var cable in request.Cables)
        {
            touchedKeys.Add(cable.Type);

            if (existingByKey.TryGetValue(cable.Type, out var existing))
            {
                existing.UpdateSpecs(request.PsuId, cable.Type, cable.CablesCount, cable.ConnectorsCount);
            }
            else
            {
                psu.AddCable(new PsuCable(
                    request.PsuId,
                    cable.Type,
                    cable.CablesCount,
                    cable.ConnectorsCount));
            }
        }

        foreach (var existing in existingByKey.Values.Where(x => !touchedKeys.Contains(x.Type)))
        {
            psu.RemoveCable(existing);
            psus.DeleteCable(existing);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.PsuCablesUpdated(logger, psu.Id);

        return [.. psu.Cables
            .Where(x => x.IsActive)
            .Select(mapper.Map<PsuCableDto>)];
    }
}
