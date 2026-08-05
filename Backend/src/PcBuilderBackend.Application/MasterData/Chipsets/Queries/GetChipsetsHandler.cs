using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetsHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetChipsetsQuery, List<ChipsetDto>>
{
    public async Task<List<ChipsetDto>> Handle(GetChipsetsQuery request, CancellationToken cancellationToken)
    {
        return await context.Chipsets.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ProjectTo<ChipsetDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}