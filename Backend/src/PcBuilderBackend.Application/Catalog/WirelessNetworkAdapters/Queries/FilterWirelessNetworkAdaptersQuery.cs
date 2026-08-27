using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public record FilterWirelessNetworkAdaptersQuery(PagedRequest<WirelessNetworkAdapterFilter> Request)
    : IRequest<PagedResult<WirelessNetworkAdapterDto>>;
