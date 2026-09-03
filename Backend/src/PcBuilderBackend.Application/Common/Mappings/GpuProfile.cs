using AutoMapper;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GpuProfile : Profile
{
    public GpuProfile()
    {
        CreateMap<Gpu, GpuDto>()
            .ForCtorParam(nameof(GpuDto.GpuSeriesId), o => o.MapFrom(s => s.SeriesId))
            .ForCtorParam(nameof(GpuDto.GpuSeriesName), o => o.MapFrom(s => s.Series.Name))
            .ForCtorParam(nameof(GpuDto.ManufacturerName), o => o.MapFrom(s => s.Manufacturer.Name))
            .ForMember(d => d.GpuSeriesId, o => o.MapFrom(s => s.SeriesId))
            .ForMember(d => d.GpuSeriesName, o => o.MapFrom(s => s.Series.Name))
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name));
    }
}
