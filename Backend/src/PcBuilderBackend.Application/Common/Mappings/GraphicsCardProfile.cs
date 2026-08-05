using AutoMapper;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GraphicsCardProfile : Profile
{
    public GraphicsCardProfile()
    {
        CreateMap<GraphicsCardPowerConnector, GraphicsCardPowerConnectorDto>();

        CreateMap<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name))
            .ForMember(d => d.GpuName, o => o.MapFrom(s => s.Gpu.Name));

        CreateMap<GraphicsCard, GraphicsCardDto>()
            .IncludeBase<GraphicsCard, GraphicsCardListItemDto>()
            .ForMember(
                d => d.PowerConnectors,
                o => o.MapFrom(s => s.PowerConnectors.Where(c => c.IsActive)));
    }
}
