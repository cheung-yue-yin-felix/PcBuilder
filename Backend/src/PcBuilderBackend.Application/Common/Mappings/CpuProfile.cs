using AutoMapper;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Mappings;

public class CpuProfile: Profile
{
    public CpuProfile()
    {
        CreateMap<CpuDto, Cpu>();
    }
}