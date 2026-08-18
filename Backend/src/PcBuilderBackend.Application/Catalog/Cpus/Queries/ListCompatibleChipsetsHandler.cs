using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class ListCompatibleChipsetsHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<ListCompatibleChipsetsQuery, List<CpuSupportChipsetDto>>
{
    public async Task<List<CpuSupportChipsetDto>> Handle(
        ListCompatibleChipsetsQuery request,
        CancellationToken cancellationToken)
    {
        return await context.CpuSupportChipsets
            .AsNoTracking()
            .Where(x => x.CpuId == request.CpuId)
            .ProjectTo<CpuSupportChipsetDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
