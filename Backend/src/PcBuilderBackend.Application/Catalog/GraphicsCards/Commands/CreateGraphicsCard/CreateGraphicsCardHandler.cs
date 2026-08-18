using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;

public class CreateGraphicsCardHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<CreateGraphicsCardHandler> logger)
    : IRequestHandler<CreateGraphicsCardCommand, GraphicsCardDto>
{
    public async Task<GraphicsCardDto> Handle(
        CreateGraphicsCardCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new GraphicsCard(
            request.Name,
            request.ManufacturerId,
            request.GpuId,
            request.VideoMemoryGb,
            request.PcieSlotsUsed,
            request.PcieGeneration,
            request.LengthMm,
            request.WidthMm,
            request.HeightMm,
            request.PowerConsumptionWatts,
            request.PowerConnectorType,
            request.PowerConnectorCount);

        context.GraphicsCards.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.GraphicsCard, entity.Id);

        return mapper.Map<GraphicsCardDto>(entity);
    }
}
