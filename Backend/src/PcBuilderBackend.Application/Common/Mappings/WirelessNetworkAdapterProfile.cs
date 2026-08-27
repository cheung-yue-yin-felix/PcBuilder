using AutoMapper;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class WirelessNetworkAdapterProfile : Profile
{
    public WirelessNetworkAdapterProfile()
    {
        CreateMap<WirelessNetworkAdapter, WirelessNetworkAdapterDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name));
    }
}
