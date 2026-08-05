using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkUpdateCpuSeries;

public class BulkUpdateCpuSeriesHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkUpdateCpuSeriesCommand, List<CpuSeriesDto>>
{
    public async Task<List<CpuSeriesDto>> Handle(BulkUpdateCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<CpuSeriesDto>();
        
        foreach (var cpuSeriesDto in request.CpuSeries)
        {
            var entity = await context.CpuSeries.FirstOrDefaultAsync(cs => cs.Id == cpuSeriesDto.Id && cs.IsActive, cancellationToken);
            
            if (entity is null) return result;
            
            entity.Rename(cpuSeriesDto.Name);
            entity.UpdateManufacturer(cpuSeriesDto.ManufacturerId);
            entity.UpdateSpecs(cpuSeriesDto.SocketId);
            
            result.Add(mapper.Map<CpuSeriesDto>(entity));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}