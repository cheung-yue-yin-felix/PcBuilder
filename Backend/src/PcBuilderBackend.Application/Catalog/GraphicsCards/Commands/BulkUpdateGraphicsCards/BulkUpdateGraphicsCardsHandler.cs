using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkUpdateGraphicsCards;

public class BulkUpdateGraphicsCardsHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkUpdateGraphicsCardsHandler> logger)
    : IRequestHandler<BulkUpdateGraphicsCardsCommand, List<GraphicsCardDto>?>
{
    public async Task<List<GraphicsCardDto>?> Handle(
        BulkUpdateGraphicsCardsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<GraphicsCardDto>();

        foreach (var item in request.Cards)
        {
            var entity = await graphicsCards.GetByIdAsync(item.Id, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(new GraphicsCardSpecs
            {
                GpuId = item.GpuId,
                VideoMemoryGb = item.VideoMemoryGb,
                PcieSlotsUsed = item.PcieSlotsUsed,
                PcieGeneration = item.PcieGeneration,
                IsLowProfile = item.IsLowProfile,
                LengthMm = item.LengthMm,
                WidthMm = item.WidthMm,
                HeightMm = item.HeightMm,
                PowerConsumptionWatts = item.PowerConsumptionWatts,
                PowerConnectorType = item.PowerConnectorType,
                PowerConnectorCount = item.PowerConnectorCount
            });

            result.Add(mapper.Map<GraphicsCardDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.GraphicsCard);
        return result;
    }
}
