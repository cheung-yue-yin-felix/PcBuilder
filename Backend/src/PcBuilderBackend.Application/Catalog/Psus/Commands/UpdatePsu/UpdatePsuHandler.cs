using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

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
        entity.UpdateSpecs(new PsuSpecs
        {
            Wattage = request.Wattage,
            Modularity = request.Modularity,
            FormFactor = request.FormFactor,
            LengthMm = request.LengthMm,
            WidthMm = request.WidthMm,
            HeightMm = request.HeightMm
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.Psu, entity.Id);

        return mapper.Map<PsuDto>(entity);
    }
}
