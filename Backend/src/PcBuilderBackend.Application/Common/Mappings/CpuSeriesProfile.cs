using AutoMapper;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class CpuSeriesProfile : Profile
{
    public CpuSeriesProfile()
    {
        CreateMap<CpuSeries, CpuSeriesDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name))
            .ForMember(dest => dest.SocketName, opt => opt.MapFrom(src => src.Socket.Name));
    }
}