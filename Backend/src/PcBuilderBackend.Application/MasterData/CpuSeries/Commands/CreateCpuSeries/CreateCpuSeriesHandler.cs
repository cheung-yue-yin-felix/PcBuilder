using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;

namespace PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;

public class CreateCpuSeriesHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<CreateCpuSeriesCommand, CpuSeriesDto>
{
    public async Task<CpuSeriesDto> Handle(CreateCpuSeriesCommand request, CancellationToken cancellationToken)
    {
        var cpuSeries = new Domain.Entities.CpuSeries(request.ManufacturerId, request.SocketId, request.Name);
        context.CpuSeries.Add(cpuSeries);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<CpuSeriesDto>(cpuSeries);
    }
}