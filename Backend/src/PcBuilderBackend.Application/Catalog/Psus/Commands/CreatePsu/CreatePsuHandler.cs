using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.CreatePsu;

public class CreatePsuHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<CreatePsuHandler> logger)
    : IRequestHandler<CreatePsuCommand, PsuDto>
{
    public async Task<PsuDto> Handle(CreatePsuCommand request, CancellationToken cancellationToken)
    {
        var entity = new Psu(
            request.Name,
            request.ManufacturerId,
            request.Wattage,
            request.Modularity,
            request.FormFactor,
            request.LengthMm,
            request.WidthMm,
            request.HeightMm);

        foreach (var cable in request.Cables)
        {
            entity.AddCable(new PsuCable(entity.Id, cable.Type, cable.CablesCount, cable.ConnectorsCount));
        }

        context.Psus.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        EntityLog.Created(logger, EntityLog.Psu, entity.Id);

        return mapper.Map<PsuDto>(entity);
    }
}
