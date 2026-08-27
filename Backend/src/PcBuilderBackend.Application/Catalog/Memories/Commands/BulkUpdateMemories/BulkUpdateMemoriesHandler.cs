using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkUpdateMemories;

public class BulkUpdateMemoriesHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BulkUpdateMemoriesCommand, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(BulkUpdateMemoriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<RamDto>();
        foreach (var command in request.Memories)
        {
            var entity = await memories.GetByIdAsync(command.Id, cancellationToken);
            if (entity is null) return result;

            entity.Rename(command.Name);
            entity.UpdateManufacturer(command.ManufacturerId);
            entity.UpdateSpecs(
                command.Color,
                command.DdrGeneration,
                command.RamFormFactor,
                command.RamRank,
                command.MemorySizePerStickGb,
                command.TotalMemorySizeGb,
                command.ModulesCount,
                command.MaxMemorySpeedMts,
                command.HeightMm
            );
            result.Add(mapper.Map<RamDto>(entity));
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}