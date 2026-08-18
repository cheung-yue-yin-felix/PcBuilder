using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;

public class BulkCreateGraphicsCardsHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkCreateGraphicsCardsHandler> logger)
    : IRequestHandler<BulkCreateGraphicsCardsCommand, List<GraphicsCardDto>>
{
    public async Task<List<GraphicsCardDto>> Handle(
        BulkCreateGraphicsCardsCommand request,
        CancellationToken cancellationToken)
    {
        var result = new List<GraphicsCardDto>();

        foreach (var item in request.Cards)
        {
            var entity = new GraphicsCard(
                item.Name,
                item.ManufacturerId,
                item.GpuId,
                item.VideoMemoryGb,
                item.PcieSlotsUsed,
                item.PcieGeneration,
                item.LengthMm,
                item.WidthMm,
                item.HeightMm,
                item.PowerConsumptionWatts,
                item.PowerConnectorType,
                item.PowerConnectorCount);

            context.GraphicsCards.Add(entity);
            result.Add(mapper.Map<GraphicsCardDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.GraphicsCard);
        return result;
    }
}
