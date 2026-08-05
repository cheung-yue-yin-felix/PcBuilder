using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.MasterData.Gpus.Queries;

public record GetGpusQuery(int PageIndex = 0, int PageSize = 10, string? Name = "", Guid? ManufacturerId = null, Guid? GpuSeriesId = null) : IRequest<PagedResult<GpuDto>>;