using MediatR;
using PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Dto;

namespace PcBuilderBackend.Application.Catalog.WiredNetworkAdapters.Queries;

public record GetWiredNetworkAdapterByIdQuery(Guid Id) : IRequest<WiredNetworkAdapterDto?>;
