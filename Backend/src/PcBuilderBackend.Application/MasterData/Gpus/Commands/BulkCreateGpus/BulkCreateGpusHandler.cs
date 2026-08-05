using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;

public class BulkCreateGpusHandler(IApplicationDbContext context, IMapper mapper, ILogger<BulkCreateGpusHandler> logger)
    : IRequestHandler<BulkCreateGpusCommand, List<GpuDto>>
{
    public async Task<List<GpuDto>> Handle(BulkCreateGpusCommand request, CancellationToken cancellationToken)
    {
        var result = new List<GpuDto>();
        
        foreach (var gpu in request.Gpus.Select(g => new Domain.Entities.Gpu(g.Name, g.ManufacturerId, g.GpuSeriesId)))
        {
            context.Gpus.Add(gpu);
            result.Add(mapper.Map<GpuDto>(gpu));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        EntityLog.BulkCreated(logger, result.Count, EntityLog.Gpu);
        return result;
    }
}