using AutoMapper;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class GpuProfile : Profile
{
    public GpuProfile()
    {
        CreateMap<Gpu, GpuDto>()
            .ForMember(d => d.GpuSeriesId, o => o.MapFrom(s => s.SeriesId));
    }
}
