using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

public class GetCpuSeriesByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCpuSeriesByIdQuery, CpuSeriesDto?>
{
    public async Task<CpuSeriesDto?> Handle(GetCpuSeriesByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.CpuSeries
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CpuSeriesId && x.IsActive, cancellationToken);

        return result == null ? null : mapper.Map<CpuSeriesDto>(result);
    }
}