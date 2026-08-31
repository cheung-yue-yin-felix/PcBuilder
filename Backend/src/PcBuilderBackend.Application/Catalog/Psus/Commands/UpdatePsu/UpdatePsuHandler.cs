using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.UpdatePsu;

public class UpdatePsuHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdatePsuHandler> logger)
    : IRequestHandler<UpdatePsuCommand, PsuDto?>
{
    public async Task<PsuDto?> Handle(UpdatePsuCommand request, CancellationToken cancellationToken)
    {
        var entity = await psus.GetWithChildrenAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.Psu, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.Wattage,
            request.Modularity,
            request.FormFactor,
            request.LengthMm,
            request.WidthMm,
            request.HeightMm);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.Psu, entity.Id);

        return mapper.Map<PsuDto>(entity);
    }
}
