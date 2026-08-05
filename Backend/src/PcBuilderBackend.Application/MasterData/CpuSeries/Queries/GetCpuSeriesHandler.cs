using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetCpuSeriesQuery, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(GetCpuSeriesQuery request, CancellationToken cancellationToken)
    {
        return await context.CpuSeries
            .AsNoTracking()
            .Where(cs => cs.IsActive)
            .ProjectTo<CpuSeriesDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}