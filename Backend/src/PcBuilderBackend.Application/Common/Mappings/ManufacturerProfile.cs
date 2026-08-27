using AutoMapper;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class ManufacturerProfile : Profile
{
    public ManufacturerProfile()
    {
        CreateMap<Manufacturer, ManufacturerDto>();
    }
}