using MediatR;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Queries;

public record GetManufacturerByIdQuery(System.Guid Id) : IRequest<ManufacturerDto?>;
