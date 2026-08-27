using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;

public class CreateMemoryHandler(IRamRepository memories, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateMemoryCommand, RamDto>
{
    public async Task<RamDto> Handle(CreateMemoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new Ram(
            request.Name,
            request.ManufacturerId,
            request.Color,
            request.DdrGeneration,
            request.RamFormFactor,
            request.RamRank,
            request.MemorySizePerStickGb,
            request.TotalMemorySizeGb,
            request.ModulesCount,
            request.MaxMemorySpeedMts,
            request.HeightMm);

        memories.Add(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<RamDto>(entity);
    }
}
