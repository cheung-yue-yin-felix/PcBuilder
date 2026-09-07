using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;
using PcBuilderBackend.Application.Common.Validation;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Psus.Commands.ImportPsus;

public class ImportPsusHandler(
    IPsuRepository psus,
    IUnitOfWork unitOfWork,
    IActiveEntityLookup lookup,
    IExcelImportService excel,
    IMapper mapper,
    ILogger<ImportPsusHandler> logger)
    : IRequestHandler<ImportPsusCommand, List<PsuDto>>
{
    public async Task<List<PsuDto>> Handle(ImportPsusCommand request, CancellationToken cancellationToken)
    {
        var rows = await excel.ParsePsuImportAsync(request.Stream, cancellationToken);

        await ActiveEntityGuard.EnsureManufacturersExist(
            lookup, rows.Select(r => r.ManufacturerId), cancellationToken);

        var result = new List<Psu>();

        foreach (var row in rows)
        {
            var entity = new Psu(
                row.Name,
                row.ManufacturerId,
                new PsuSpecs
                {
                    Wattage = row.Wattage,
                    Modularity = row.Modularity,
                    FormFactor = row.FormFactor,
                    LengthMm = row.LengthMm,
                    WidthMm = row.WidthMm,
                    HeightMm = row.HeightMm
                });

            foreach (var cable in row.Cables)
            {
                entity.AddCable(new PsuCable(entity.Id, cable.Type, cable.CablesCount, cable.ConnectorsCount));
            }

            psus.Add(entity);
            result.Add(entity);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        EntityLog.Imported(logger, result.Count, EntityLog.Psu);

        return [.. result.Select(mapper.Map<PsuDto>)];
    }
}
