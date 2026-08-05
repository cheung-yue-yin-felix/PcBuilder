using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

public class GetGpuSeriesByIdHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetGpuSeriesByIdQuery, GpuSeriesDto?>
{
    public async Task<GpuSeriesDto?> Handle(GetGpuSeriesByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.GpuSeries
            .Where(g => g.Id == request.Id && g.IsActive)
            .ProjectTo<GpuSeriesDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}