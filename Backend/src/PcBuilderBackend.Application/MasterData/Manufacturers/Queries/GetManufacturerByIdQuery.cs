using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public record GetManufacturerByIdQuery(Guid Id) : IRequest<ManufacturerDto?>;
