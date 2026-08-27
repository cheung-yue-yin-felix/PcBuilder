using AutoMapper;
using PcBuilderBackend.Application.MasterData.Sockets.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class SocketProfile : Profile
{
    public SocketProfile()
    {
        CreateMap<Socket, SocketDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.Name : string.Empty));
    }
}