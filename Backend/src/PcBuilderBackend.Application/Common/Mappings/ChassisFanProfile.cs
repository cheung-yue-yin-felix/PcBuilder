using AutoMapper;
using PcBuilderBackend.Application.Catalog.ChassisFans.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class ChassisFanProfile : Profile
{
    public ChassisFanProfile()
    {
        CreateMap<ChassisFan, ChassisFanDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name));
    }
}
