using MediatR;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using System.Collections.Generic;

namespace PcBuilderBackend.Application.Catalog.Manufacturers.Queries;

public record GetManufacturersQuery() : IRequest<List<ManufacturerDto>>;
