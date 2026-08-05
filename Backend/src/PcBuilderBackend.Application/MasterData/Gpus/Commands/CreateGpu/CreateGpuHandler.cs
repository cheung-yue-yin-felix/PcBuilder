using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.Common.Logging;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;

public class CreateGpuHandler(IApplicationDbContext context, IMapper mapper, ILogger<CreateGpuHandler> logger) : IRequestHandler<CreateGpuCommand, GpuDto>
{
    public async Task<GpuDto> Handle(CreateGpuCommand request, CancellationToken cancellationToken)
    {
        var gpu = new Domain.Entities.Gpu(request.Name, request.ManufacturerId, request.GpuSeriesId);
        context.Gpus.Add(gpu);
        await context.SaveChangesAsync(cancellationToken);
        EntityLog.Created(logger, EntityLog.Gpu, gpu.Id);
        return mapper.Map<GpuDto>(gpu);
    }
}