using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerHandler(IApplicationDbContext context, IMapper mapper): IRequestHandler<CreateManufacturerCommand, ManufacturerDto>
{
    public async Task<ManufacturerDto> Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var entity = new Manufacturer(request.Name);
        context.Manufacturers.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return mapper.Map<ManufacturerDto>(entity);
    }
}