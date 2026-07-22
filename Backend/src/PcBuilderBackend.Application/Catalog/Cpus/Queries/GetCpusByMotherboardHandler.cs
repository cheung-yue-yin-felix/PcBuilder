using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public class GetCpusByMotherboardHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<GetCpusByMotherboardQuery, List<CpuDto>>
{
    public async Task<List<CpuDto>> Handle(GetCpusByMotherboardQuery request, CancellationToken cancellationToken)
    {
        var motherboard = context.Motherboards.AsNoTracking()
            .FirstOrDefault(x => x.Id == request.MotherboardId && x.IsActive);

        if (motherboard == null) return new List<CpuDto>();
        
        return await context.Cpus.AsNoTracking()
            .Where(x => x.SocketId == motherboard.SocketId && x.DdrGeneration == motherboard.DdrGeneration)
            .OrderBy(x => x.Name)
            .ProjectTo<CpuDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}