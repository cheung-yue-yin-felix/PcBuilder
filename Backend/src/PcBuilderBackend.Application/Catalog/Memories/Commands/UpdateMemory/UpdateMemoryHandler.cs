using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;

public class UpdateMemoryHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateMemoryCommand, RamDto?>
{
    public async Task<RamDto?> Handle(UpdateMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await memories.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) return null;

        entity.Rename(request.Name);
        entity.UpdateManufacturer(request.ManufacturerId);
        entity.UpdateSpecs(new RamSpecs
        {
            Color = request.Color,
            DdrGeneration = request.DdrGeneration,
            RamFormFactor = request.RamFormFactor,
            RamRank = request.RamRank,
            MemorySizePerStickGb = request.MemorySizePerStickGb,
            TotalMemorySizeGb = request.TotalMemorySizeGb,
            ModulesCount = request.ModulesCount,
            MaxMemorySpeedMts = request.MaxMemorySpeedMts,
            HeightMm = request.HeightMm
        });

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<RamDto>(entity);
    }
}
