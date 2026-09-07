using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;

public class CreateGraphicsCardHandler(
    IGraphicsCardRepository graphicsCards,
    IUnitOfWork unitOfWork,
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
            new GraphicsCardSpecs
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

        graphicsCards.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.GraphicsCard, entity.Id);

        return mapper.Map<GraphicsCardDto>(entity);
    }
}
