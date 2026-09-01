using AutoMapper;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GraphicsCardProfile : Profile
{
    public GraphicsCardProfile()
    {
        CreateMap<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name))
            .ForMember(d => d.GpuName, o => o.MapFrom(s => s.Gpu.Name));

        CreateMap<GraphicsCard, GraphicsCardDto>()
            .IncludeBase<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(d => d.GpuManufacturerId, o => o.MapFrom(s => s.Gpu.ManufacturerId))
            .ForMember(d => d.GpuManufacturerName, o => o.MapFrom(s => s.Gpu.Manufacturer.Name))
            .ForMember(d => d.GpuSeriesId, o => o.MapFrom(s => s.Gpu.SeriesId))
            .ForMember(d => d.GpuSeriesName, o => o.MapFrom(s => s.Gpu.Series.Name));
    }
}
