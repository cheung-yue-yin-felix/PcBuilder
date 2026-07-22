using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;

public class CreateCpuHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<CreateCpuCommand, CpuDto>
{
    public async Task<CpuDto> Handle(CreateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = new Cpu(request.Name, request.ManufacturerId, request.SocketId, request.SeriesId, request.DdrGeneration,
            request.MaxMemoryGb, request.IntegratedGraphics, request.IncludedStockCooler,
            request.ThermalDesignPower);
        
        context.Cpus.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<CpuDto>(entity);
    }
}