using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkCreateMemories;

public class BulkCreateMemoriesHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BulkCreateMemoriesCommand, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(BulkCreateMemoriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<RamDto>();
        foreach (var entity in request.Memories.Select(memory => new Ram(
                     memory.Name,
                     memory.ManufacturerId,
                     memory.Color,
                     memory.DdrGeneration,
                     memory.RamFormFactor,
                     memory.RamRank,
                     memory.MemorySizePerStickGb,
                     memory.TotalMemorySizeGb,
                     memory.ModulesCount,
                     memory.MaxMemorySpeedMts,
                     memory.HeightMm
                 )))
        {
            memories.Add(entity);
            result.Add(mapper.Map<RamDto>(entity));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}