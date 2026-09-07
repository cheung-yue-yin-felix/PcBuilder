using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;

public class BulkCreateGraphicsCardsHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<BulkCreateGraphicsCardsHandler> logger)
    : IRequestHandler<BulkCreateGraphicsCardsCommand, List<GraphicsCardDto>>
{
    public async Task<List<GraphicsCardDto>> Handle(
        BulkCreateGraphicsCardsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<GraphicsCardDto>();

        foreach (var entity in request.Cards.Select(item => new GraphicsCard(
                     item.Name,
                     item.ManufacturerId,
                     new GraphicsCardSpecs
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
                     })))
        {
            graphicsCards.Add(entity);
            result.Add(mapper.Map<GraphicsCardDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.GraphicsCard);
        return result;
    }
}
