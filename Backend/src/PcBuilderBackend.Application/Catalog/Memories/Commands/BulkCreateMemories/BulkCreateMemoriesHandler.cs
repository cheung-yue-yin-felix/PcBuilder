using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.BulkCreateMemories;

public class BulkCreateMemoriesHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<BulkCreateMemoriesCommand, List<RamDto>>
{
    public async Task<List<RamDto>> Handle(BulkCreateMemoriesCommand request, CancellationToken cancellationToken)
    {
        var result = new List<RamDto>();
        foreach (var entity in request.Memories.Select(memory => new Ram(
                     memory.Name,
                     memory.ManufacturerId,
                     new RamSpecs
                     {
                         Color = memory.Color,
                         DdrGeneration = memory.DdrGeneration,
                         RamFormFactor = memory.RamFormFactor,
                         RamRank = memory.RamRank,
                         MemorySizePerStickGb = memory.MemorySizePerStickGb,
                         TotalMemorySizeGb = memory.TotalMemorySizeGb,
                         ModulesCount = memory.ModulesCount,
                         MaxMemorySpeedMts = memory.MaxMemorySpeedMts,
                         HeightMm = memory.HeightMm
                     })))
        {
            memories.Add(entity);
            result.Add(mapper.Map<RamDto>(entity));
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return result;
    }
}