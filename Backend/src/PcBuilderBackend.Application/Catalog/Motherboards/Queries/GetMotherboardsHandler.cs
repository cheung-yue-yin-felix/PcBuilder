using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardsQuery, List<MotherboardDto>>
{
    public async Task<List<MotherboardDto>> Handle(GetMotherboardsQuery request, CancellationToken cancellationToken)
    {
        var entities = context.Motherboards.AsNoTracking().Where(m => m.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Name))
            entities = entities.Where(x => x.Name.Contains(request.Name, StringComparison.CurrentCultureIgnoreCase));

        if (request.ManufacturerId.HasValue && request.ManufacturerId.Value != Guid.Empty)
            entities = entities.Where(x => x.ManufacturerId == request.ManufacturerId.Value);

        if (request.SocketId.HasValue && request.SocketId.Value != Guid.Empty)
            entities = entities.Where(x => x.SocketId == request.SocketId.Value);

        if (request.ChipsetId.HasValue && request.ChipsetId.Value != Guid.Empty)
            entities = entities.Where(x => x.ChipsetId == request.ChipsetId.Value);

        if (request.DdrGeneration.HasValue && Enum.IsDefined(request.DdrGeneration.Value))
            entities = entities.Where(x => x.DdrGeneration == request.DdrGeneration.Value);

        if (request.RamFormFactor.HasValue && Enum.IsDefined(request.RamFormFactor.Value))
            entities = entities.Where(x => x.RamFormFactor == request.RamFormFactor.Value);

        if (request.FormFactor.HasValue && Enum.IsDefined(request.FormFactor.Value))
            entities = entities.Where(x => x.FormFactor == request.FormFactor.Value);

        if (request.WifiEnabled.HasValue)
            entities = entities.Where(x => x.WifiEnabled == request.WifiEnabled.Value);

        if (request.BluetoothEnabled.HasValue)
            entities = entities.Where(x => x.BluetoothEnabled == request.BluetoothEnabled.Value);

        return await entities
            .OrderBy(x => x.Name)
            .ProjectTo<MotherboardDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
