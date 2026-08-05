using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkUpdateGraphicsCards;

public class BulkUpdateGraphicsCardsHandler(
    IApplicationDbContext context,
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
            var entity = await context.GraphicsCards
                .FirstOrDefaultAsync(x => x.Id == item.Id && x.IsActive, cancellationToken);

            if (entity is null)
            {
                EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, item.Id);
                return null;
            }

            entity.Rename(item.Name);
            entity.UpdateManufacturer(item.ManufacturerId);
            entity.UpdateSpecs(
                item.GpuId,
                item.VideoMemoryGb,
                item.PcieSlotsUsed,
                item.PcieGeneration,
                item.LengthMm,
                item.WidthMm,
                item.HeightMm,
                item.PowerConsumptionWatts);

            result.Add(mapper.Map<GraphicsCardDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkUpdated(logger, result.Count, EntityLog.GraphicsCard);
        return result;
    }
}
