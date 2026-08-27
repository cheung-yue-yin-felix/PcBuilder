using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public record GetWiredNetworkAdaptersQuery(PagedRequest Request) : IRequest<PagedResult<WiredNetworkAdapterDto>>;
