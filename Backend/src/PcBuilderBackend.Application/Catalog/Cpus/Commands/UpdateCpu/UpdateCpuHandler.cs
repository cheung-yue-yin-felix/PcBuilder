using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;

public class UpdateCpuHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<UpdateCpuCommand, CpuDto?>
{
    public async Task<CpuDto?> Handle(UpdateCpuCommand request, CancellationToken cancellationToken)
    {
        var entity = context.Cpus.FirstOrDefault(c => c.Id == request.Id && c.IsActive);
        if (entity == null) return null;
        entity.Rename(request.Name);
        entity.UpdateSpecs(
            request.ManufacturerId, 
            request.SocketId, 
            request.SeriesId,
            request.DdrGeneration, 
            request.MaxMemoryGb, 
            request.IntegratedGraphics, 
            request.IncludedStockCooler, 
            request.ThermalDesignPower);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<CpuDto>(entity);
    }
}