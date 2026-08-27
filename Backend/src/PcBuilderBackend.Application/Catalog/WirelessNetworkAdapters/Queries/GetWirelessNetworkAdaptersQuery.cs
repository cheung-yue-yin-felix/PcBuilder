using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public record GetWirelessNetworkAdaptersQuery(PagedRequest Request)
    : IRequest<PagedResult<WirelessNetworkAdapterDto>>;
