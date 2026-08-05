using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using System.Collections.Generic;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public record GetManufacturersQuery() : IRequest<List<ManufacturerDto>>;
