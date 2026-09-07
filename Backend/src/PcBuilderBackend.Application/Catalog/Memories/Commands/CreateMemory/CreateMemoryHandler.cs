using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;
using PcBuilderBackend.Domain.ValueObjects;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;

public class CreateMemoryHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateMemoryCommand, RamDto>
{
    public async Task<RamDto> Handle(CreateMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Ram(
            request.Name,
            request.ManufacturerId,
            new RamSpecs
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

        memories.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<RamDto>(entity);
    }
}
