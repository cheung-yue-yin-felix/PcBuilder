using MediatR;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;

namespace PcBuilderBackend.Application.Catalog.Cpus.Queries;

public record GetCpuByMotherboardQuery(Guid MotherboardId): IRequest<IEnumerable<CpuDto>>;