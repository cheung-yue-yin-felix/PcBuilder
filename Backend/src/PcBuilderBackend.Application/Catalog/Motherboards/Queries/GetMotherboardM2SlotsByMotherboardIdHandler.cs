using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Motherboards.Queries;

public class GetMotherboardM2SlotsByMotherboardIdHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetMotherboardM2SlotsByMotherboardIdQuery, List<MotherboardM2Dto>>
{
    public async Task<List<MotherboardM2Dto>> Handle(
        GetMotherboardM2SlotsByMotherboardIdQuery request,
        CancellationToken cancellationToken)
    {
        var motherboard = await context.Motherboards
            .AsNoTracking()
            .Include(m => m.M2Slots)
            .ThenInclude(s => s.FormFactors)
            .FirstOrDefaultAsync(m => m.Id == request.MotherboardId && m.IsActive, cancellationToken);

        if (motherboard is null)
            return [];

        var slots = motherboard.M2Slots
            .Where(s => s.IsActive)
            .OrderBy(s => s.Key)
            .ThenBy(s => s.PcieGeneration)
            .ToList();

        return mapper.Map<List<MotherboardM2Dto>>(slots);
    }
}
