using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;

public class CreateChipsetHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateChipsetCommand, ChipsetDto>
{
    public async Task<ChipsetDto> Handle(CreateChipsetCommand request, CancellationToken cancellationToken)
    {
        var entity = new Chipset(request.Name, request.ManufacturerId, request.SocketId);
        context.Chipsets.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<ChipsetDto>(entity);
    }
}