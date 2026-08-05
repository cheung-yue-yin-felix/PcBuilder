using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Queries;

public class GetChipsetByIdHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetChipsetByIdQuery, ChipsetDto?>
{
    public async Task<ChipsetDto?> Handle(GetChipsetByIdQuery request, CancellationToken cancellationToken)
    {
        var chipset = await context.Chipsets.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.IsActive, cancellationToken);

        return chipset == null ? null : mapper.Map<ChipsetDto>(chipset);
    }
}