using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Application.Catalog.Chassis.Dto;
using PcBuilderBackend.Application.Catalog.Chassis.Queries;
using PcBuilderBackend.Application.Common.Dto;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class ChassisEndpoints
{
    public static void MapChassisEndpoints(this IEndpointRouteBuilder routes)
    {
        var subgroup = routes.MapGroup("/catalog/chassis")
            .WithSidebarGroup("Catalog", "Chassis")
            .WithDescription("Browse, Read, Edit, Add and Delete Chassis");
        
        subgroup.MapGet("/", GetChassis)
            .Produces<PagedResult<ChassisListItemDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse Chassis")
            .WithDescription("\n    GET /catalog/chassis");

        subgroup.MapGet("/{id}", GetChassisById)
            .Produces<ChassisDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get Chassis by ID")
            .WithDescription("\n    GET /catalog/chassis/{id}");
        
        subgroup.MapPost("/query", QueryChassis)
            .Produces<PagedResult<ChassisListItemDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Filter Chassis")
            .WithDescription("\n    POST /catalog/chassis/query");
    }

    private static async Task<Ok<PagedResult<ChassisListItemDto>>> GetChassis(
        [FromServices] ISender sender,
        [AsParameters] PagedRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetChassisQuery(request);
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    private static async Task<Ok<PagedResult<ChassisListItemDto>>> QueryChassis(
        [FromServices] ISender sender,
        [FromBody] PagedRequest<ChassisFilter> request,
        CancellationToken cancellationToken)
    {
        var query = new FilterChassisQuery(request);
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }

    private static async Task<Ok<ChassisDto>> GetChassisById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetChassisByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }
}