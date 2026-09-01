using AutoMapper;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class CpuCoolerProfile : Profile
{
    public CpuCoolerProfile()
    {
        CreateMap<CpuCoolerSocket, CpuCoolerSocketDto>()
            .ForMember(
                dest => dest.SocketName,
                opt => opt.MapFrom(src => src.Socket.Name));

        CreateMap<CpuCooler, CpuCoolerListItemDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name));

        CreateMap<CpuCooler, CpuCoolerDto>()
            .IncludeBase<CpuCooler, CpuCoolerListItemDto>()
            .ForMember(
                dest => dest.Sockets,
                opt => opt.MapFrom(src => src.CpuCoolerSockets.Where(s => s.IsActive)));
    }
}
