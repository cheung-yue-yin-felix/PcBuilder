using AutoMapper;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class WiredNetworkAdapterProfile : Profile
{
    public WiredNetworkAdapterProfile()
    {
        CreateMap<WiredNetworkAdapter, WiredNetworkAdapterDto>()
            .ForMember(d => d.ManufacturerName, o => o.MapFrom(s => s.Manufacturer.Name));
    }
}
