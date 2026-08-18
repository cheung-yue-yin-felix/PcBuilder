using AutoMapper;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class ChipsetProfile : Profile
{
    public ChipsetProfile()
    {
        CreateMap<Chipset, ChipsetDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name))
            .ForMember(dest => dest.SocketName, opt => opt.MapFrom(src => src.Socket.Name));
    }
}