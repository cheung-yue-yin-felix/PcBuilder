using MediatR;
using PcBuilderBackend.Application.Build.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Build.Queries;

public record ListPublicPcBuildsQuery(PagedRequest Request) : IRequest<PagedResult<PcBuildListItemDto>>;