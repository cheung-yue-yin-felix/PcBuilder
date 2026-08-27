using AutoMapper;
using PcBuilderBackend.Application.Catalog.Psus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class PsuProfile : Profile
{
    public PsuProfile()
    {
        CreateMap<PsuCable, PsuCableDto>();

        CreateMap<Psu, PsuListItemDto>()
            .ForMember(
                d => d.ManufacturerName,
                o => o.MapFrom(s => s.Manufacturer == null ? string.Empty : s.Manufacturer.Name));

        CreateMap<Psu, PsuDto>()
            .IncludeBase<Psu, PsuListItemDto>()
            .ForMember(d => d.Cables, o => o.MapFrom(s => s.Cables.Where(x => x.IsActive)));
    }
}
