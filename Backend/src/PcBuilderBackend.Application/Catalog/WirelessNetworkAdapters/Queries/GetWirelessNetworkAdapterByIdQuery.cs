using MediatR;
using PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WirelessNetworkAdapters.Queries;

public record GetWirelessNetworkAdapterByIdQuery(Guid Id) : IRequest<WirelessNetworkAdapterDto?>;
