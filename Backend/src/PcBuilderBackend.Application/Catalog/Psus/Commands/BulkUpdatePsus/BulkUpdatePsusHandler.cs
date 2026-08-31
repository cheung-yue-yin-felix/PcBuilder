using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkUpdatePsus;

public class BulkUpdatePsusHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdatePsusHandler> logger)
    : IRequestHandler<BulkUpdatePsusCommand, List<PsuDto>?>
{
    public async Task<List<PsuDto>?> Handle(BulkUpdatePsusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<PsuDto>();

        foreach (var item in request.Psus)
        {
            var entity = await psus.GetWithChildrenAsync(item.Id, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.Psu, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(
                item.Wattage,
                item.Modularity,
                item.FormFactor,
                item.LengthMm,
                item.WidthMm,
                item.HeightMm);

            result.Add(mapper.Map<PsuDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.Psu);
        return result;
    }
}
