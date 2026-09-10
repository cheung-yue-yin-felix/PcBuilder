using MediatR;
using PcBuilderBackend.Application.MasterData.Manufacturers.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Application.MasterData.Manufacturers.Queries;

public record GetManufacturersByProductTypeQuery(ProductType ProductType)
    : IRequest<List<ManufacturerDto>>;
