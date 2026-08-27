using MediatR;
using PcBuilderBackend.Application.Catalog.CpuCoolers.Dto;

namespace PcBuilderBackend.Application.Catalog.CpuCoolers.Queries;

public record GetCpuCoolerByIdQuery(Guid Id) : IRequest<CpuCoolerDto?>;
