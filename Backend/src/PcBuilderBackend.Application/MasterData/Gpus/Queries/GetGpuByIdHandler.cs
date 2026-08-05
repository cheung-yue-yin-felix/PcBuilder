using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public class GetGpuByIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetGpuByIdQuery, GpuDto?>
{
    public async Task<GpuDto?> Handle(GetGpuByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Gpus
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.IsActive)
            .ProjectTo<GpuDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
