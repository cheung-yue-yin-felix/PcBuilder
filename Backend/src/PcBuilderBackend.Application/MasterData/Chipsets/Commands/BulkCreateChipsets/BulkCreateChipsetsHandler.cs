using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;

public class BulkCreateChipsetsHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<BulkCreateChipsetsCommand, List<ChipsetDto>>
{
    public async Task<List<ChipsetDto>> Handle(BulkCreateChipsetsCommand request, CancellationToken cancellationToken)
    {
        var result = new List<ChipsetDto>();
        
        foreach (var entity in request.Chipsets.Select(dto => new Chipset(dto.Name, dto.ManufacturerId, dto.SocketId)))
        {
            context.Chipsets.Add(entity);
            result.Add(mapper.Map<ChipsetDto>(entity));
        }
        
        await context.SaveChangesAsync(cancellationToken);
        return result;
    }
}