using AutoMapper;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class CpuSeriesProfile : Profile
{
    public CpuSeriesProfile()
    {
        CreateMap<CpuSeries, CpuSeriesDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.Name : string.Empty))
            .ForMember(dest => dest.SocketName, opt => opt.MapFrom(src => src.Socket != null ? src.Socket.Name : string.Empty));
    }
}