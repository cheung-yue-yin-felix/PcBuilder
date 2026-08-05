using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;

public class UpdateGraphicsCardHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<UpdateGraphicsCardHandler> logger)
    : IRequestHandler<UpdateGraphicsCardCommand, GraphicsCardDto?>
{
    public async Task<GraphicsCardDto?> Handle(
        UpdateGraphicsCardCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await context.GraphicsCards
            .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsActive, cancellationToken);

        if (entity is null)
        {
            EntityLog.NotFoundOrInactive(logger, EntityLog.GraphicsCard, request.Id);
            return null;
        }

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(
            request.GpuId,
            request.VideoMemoryGb,
            request.PcieSlotsUsed,
            request.PcieGeneration,
            request.LengthMm,
            request.WidthMm,
            request.HeightMm,
            request.PowerConsumptionWatts);

        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Updated(logger, EntityLog.GraphicsCard, entity.Id);

        return mapper.Map<GraphicsCardDto>(entity);
    }
}
