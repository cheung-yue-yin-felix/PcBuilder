using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;

public class UpdateGraphicsCardHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<UpdateGraphicsCardHandler> logger)
    : IRequestHandler<UpdateGraphicsCardCommand, GraphicsCardDto?>
{
    public async Task<GraphicsCardDto?> Handle(
        UpdateGraphicsCardCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await graphicsCards.GetByIdAsync(request.Id, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(new GraphicsCardSpecs
        {
            GpuId = request.GpuId,
            VideoMemoryGb = request.VideoMemoryGb,
            PcieSlotsUsed = request.PcieSlotsUsed,
            PcieGeneration = request.PcieGeneration,
            IsLowProfile = request.IsLowProfile,
            LengthMm = request.LengthMm,
            WidthMm = request.WidthMm,
            HeightMm = request.HeightMm,
            PowerConsumptionWatts = request.PowerConsumptionWatts,
            PowerConnectorType = request.PowerConnectorType,
            PowerConnectorCount = request.PowerConnectorCount
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.GraphicsCard, entity.Id);

        return mapper.Map<GraphicsCardDto>(entity);
    }
}
