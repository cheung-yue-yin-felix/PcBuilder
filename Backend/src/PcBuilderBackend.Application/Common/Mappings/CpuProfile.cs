using AutoMapper;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class CpuProfile : Profile
{
    public CpuProfile()
    {
        CreateMap<CpuRamCompat, CpuRamCompatDto>();

        CreateMap<CpuSupportChipset, CpuSupportChipsetDto>()
            .ForMember(
                dest => dest.ChipsetName,
                opt => opt.MapFrom(src => src.Chipset.Name));

        CreateMap<Cpu, CpuListItemDto>()
            .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer.Name))
            .ForMember(dest => dest.SocketName, opt => opt.MapFrom(src => src.Socket.Name))
            .ForMember(dest => dest.SeriesName, opt => opt.MapFrom(src => src.Series.Name));

        CreateMap<Cpu, CpuDto>()
            .IncludeBase<Cpu, CpuListItemDto>()
            .ForMember(
                dest => dest.RamCompats,
                opt => opt.MapFrom(src => src.RamCompats.Where(s => s.IsActive)))
            .ForMember(
                dest => dest.SupportChipsets,
                opt => opt.MapFrom(src => src.SupportedChipsets.Where(s => s.IsActive)));
    }
}
