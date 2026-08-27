using AutoMapper;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GpuProfile : Profile
{
    public GpuProfile()
    {
        CreateMap<Gpu, GpuDto>()
            .ForMember(d => d.GpuSeriesId, o => o.MapFrom(s => s.SeriesId))
            .ForMember(d => d.GpuSeriesName, o => o.MapFrom(s => s.Series != null ? s.Series.Name : string.Empty))
            .ForMember(d => d.ManufacturerId, o => o.MapFrom(s => s.ManufacturerId))
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer != null ? s.Manufacturer.Name : string.Empty));
    }
}
