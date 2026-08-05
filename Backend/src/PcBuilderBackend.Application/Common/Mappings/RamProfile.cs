using AutoMapper;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class RamProfile : Profile
{
    public RamProfile()
    {
        CreateMap<Ram, RamDto>();
    }
}
