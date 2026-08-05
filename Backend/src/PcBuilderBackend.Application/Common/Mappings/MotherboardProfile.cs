using AutoMapper;
using PcBuilderBackend.Application.Catalog.Motherboards.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class MotherboardProfile : Profile
{
    public MotherboardProfile()
    {
        CreateMap<Motherboard, MotherboardDto>();
        CreateMap<MotherboardPcie, MotherboardPcieDto>();
        CreateMap<MotherboardM2, MotherboardM2Dto>()
            .ForMember(dest => dest.FormFactors, opt => opt.MapFrom(src => src.FormFactors.Select(f => f.FormFactor).ToList()));
    }
}
