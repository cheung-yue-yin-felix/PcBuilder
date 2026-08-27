using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.BulkCreatePsus;

public class BulkCreatePsusHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ILogger<BulkCreatePsusHandler> logger)
    : IRequestHandler<BulkCreatePsusCommand, List<PsuDto>>
{
    public async Task<List<PsuDto>> Handle(BulkCreatePsusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<PsuDto>();

        foreach (var item in request.Psus)
        {
            var entity = new Psu(
                item.Name,
                item.ManufacturerId,
                item.Wattage,
                item.Modularity,
                item.FormFactor,
                item.LengthMm,
                item.WidthMm,
                item.HeightMm);

            foreach (var cable in item.Cables)
            {
                entity.AddCable(new PsuCable(entity.Id, cable.Type, cable.CablesCount, cable.ConnectorsCount));
            }

            context.Psus.Add(entity);
            result.Add(mapper.Map<PsuDto>(entity));
        }

        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Psu);
        return result;
    }
}
