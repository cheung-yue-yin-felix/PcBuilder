using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardPcieSlotsByMotherboardIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardPcieSlotsByMotherboardIdQuery, List<MotherboardPcieDto>>
{
    public async Task<List<MotherboardPcieDto>> Handle(
        GetMotherboardPcieSlotsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.PcieSlots)
            .FirstOrDefaultAsync(m => m.Id == request.MotherboardId && m.IsActive, cancellationToken);

        if (motherboard is null)
            return [];

        var slots = motherboard.PcieSlots
            .Where(s => s.IsActive)
            .OrderBy(s => s.SlotType)
            .ThenBy(s => s.SlotLanes)
            .ThenBy(s => s.Generation)
            .ToList();

        return mapper.Map<List<MotherboardPcieDto>>(slots);
    }
}
