using AutoMapper;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GraphicsCardProfile : Profile
{
    public GraphicsCardProfile()
    {
        CreateMap<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer != null ? s.Manufacturer.Name : string.Empty))
            .ForMember(d => d.GpuName, o => o.MapFrom(s => s.Gpu != null ? s.Gpu.Name : string.Empty));

        CreateMap<GraphicsCard, GraphicsCardDto>()
            .IncludeBase<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(d => d.GpuManufacturerId, o => o.MapFrom(s => s.Gpu != null ? s.Gpu.ManufacturerId : Guid.Empty))
            .ForMember(d => d.GpuManufacturerName, o => o.MapFrom(s => s.Gpu != null && s.Gpu.Manufacturer != null ? s.Gpu.Manufacturer.Name : string.Empty))
            .ForMember(d => d.GpuSeriesId, o => o.MapFrom(s => s.Gpu != null ? s.Gpu.SeriesId : Guid.Empty))
            .ForMember(d => d.GpuSeriesName, o => o.MapFrom(s => s.Gpu != null && s.Gpu.Series != null ? s.Gpu.Series.Name : string.Empty));
    }
}
