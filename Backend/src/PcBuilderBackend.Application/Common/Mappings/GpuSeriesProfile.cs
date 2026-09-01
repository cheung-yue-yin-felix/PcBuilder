using AutoMapper;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GpuSeriesProfile : Profile
{
    public GpuSeriesProfile()
    {
        CreateMap<GpuSeries, GpuSeriesDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name));
    }
}