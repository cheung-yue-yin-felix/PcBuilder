using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class ListCompatibleMemoriesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<ListCompatibleMemoriesQuery, List<CpuRamCompatDto>>
{
    public async Task<List<CpuRamCompatDto>> Handle(
        ListCompatibleMemoriesQuery request,
        CancellationToken cancellationToken)
    {
        return await context.CpuRamCompats
            .AsNoTracking()
            .Where(x => x.CpuId == request.CpuId && x.IsActive)
            .OrderBy(x => x.DdrGeneration)
            .ThenBy(x => x.RamModuleCount)
            .ThenBy(x => x.RamRank)
            .ProjectTo<CpuRamCompatDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
