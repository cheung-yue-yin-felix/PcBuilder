using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpusByMotherboardHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetCpusByMotherboardQuery, List<CpuListItemDto>>
{
    public async Task<List<CpuListItemDto>> Handle(
        GetCpusByMotherboardQuery request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.MotherboardId && x.IsActive, cancellationToken);

        if (motherboard == null)
            return [];

        return await context.Cpus.AsNoTracking()
            .Where(x =>
                x.IsActive &&
                x.SocketId == motherboard.SocketId &&
                x.RamCompats.Any(r => r.IsActive && r.DdrGeneration == motherboard.DdrGeneration))
            .OrderBy(x => x.Name)
            .ProjectTo<CpuListItemDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
