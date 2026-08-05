using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkCreateGraphicsCards;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkDeleteGraphicsCards;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.BulkUpdateGraphicsCards;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.CreateGraphicsCard;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.DeleteGraphicsCard;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Commands.UpdateGraphicsCard;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Dto;
using PcBuilderBackend.Application.Catalog.GraphicsCards.Queries;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class GraphicsCardEndpoints
{
    public static void MapGraphicsCardEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("graphics-card")
            .WithSidebarGroup("Catalog", "Graphics Card")
            .WithDescription("Browse, Read, Edit, Add and Delete graphics cards");

        subgroup.MapGet("/", GetGraphicsCards)
            .Produces<PagedResult<GraphicsCardListItemDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse graphics cards")
            .WithDescription("\n    GET /catalog/graphics-card");

        subgroup.MapGet("/{id:guid}", GetGraphicsCardById)
            .Produces<GraphicsCardDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read graphics card by ID")
            .WithDescription("\n    GET /catalog/graphics-card/{id}");

        subgroup.MapPost("/", CreateGraphicsCard)
            .Produces<GraphicsCardDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a graphics card")
            .WithDescription("\n    POST /catalog/graphics-card");

        subgroup.MapPost("/bulk", BulkCreateGraphicsCards)
            .Produces<List<GraphicsCardDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple graphics cards")
            .WithDescription("\n    POST /catalog/graphics-card/bulk");

        subgroup.MapPut("/", UpdateGraphicsCard)
            .Produces<GraphicsCardDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a graphics card")
            .WithDescription("\n    PUT /catalog/graphics-card");

        subgroup.MapPut("/bulk", BulkUpdateGraphicsCards)
            .Produces<List<GraphicsCardDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple graphics cards")
            .WithDescription("\n    PUT /catalog/graphics-card/bulk");

        subgroup.MapDelete("/{id:guid}", DeleteGraphicsCard)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a graphics card")
            .WithDescription("\n    DELETE /catalog/graphics-card/{id}");

        subgroup.MapDelete("/bulk", BulkDeleteGraphicsCards)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple graphics cards")
            .WithDescription("\n    DELETE /catalog/graphics-card/bulk");
    }

    private static async Task<Ok<PagedResult<GraphicsCardListItemDto>>> GetGraphicsCards(
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] string? name,
        [FromQuery] Guid? manufacturerId,
        [FromQuery] Guid? gpuId,
        [FromQuery] PcieGeneration? pcieGeneration,
        [FromQuery] int? minVideoMemoryGb,
        [FromQuery] int? maxVideoMemoryGb,
        [FromQuery] decimal? maxLengthMm,
        [FromQuery] decimal? maxPowerConsumptionWatts,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetGraphicsCardsQuery(
            pageIndex,
            pageSize,
            name,
            manufacturerId,
            gpuId,
            pcieGeneration,
            minVideoMemoryGb,
            maxVideoMemoryGb,
            maxLengthMm,
            maxPowerConsumptionWatts);

        return TypedResults.Ok(await sender.Send(query, cancellationToken));
    }

    private static async Task<Results<Ok<GraphicsCardDto>, NotFound>> GetGraphicsCardById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetGraphicsCardByIdQuery(id), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Created<GraphicsCardDto>> CreateGraphicsCard(
        [Validate] [FromBody] CreateGraphicsCardCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}/{result.Id}", result);
    }

    private static async Task<Results<Ok<GraphicsCardDto>, NotFound>> UpdateGraphicsCard(
        [Validate] [FromBody] UpdateGraphicsCardCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteGraphicsCard(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteGraphicsCardCommand(id), cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Results<Created<List<GraphicsCardDto>>, BadRequest>> BulkCreateGraphicsCards(
        [Validate] [FromBody] BulkCreateGraphicsCardsCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count == 0
            ? TypedResults.BadRequest()
            : TypedResults.Created($"{httpContext.Request.Path}", result);
    }

    private static async Task<Results<Ok<List<GraphicsCardDto>>, BadRequest>> BulkUpdateGraphicsCards(
        [Validate] [FromBody] BulkUpdateGraphicsCardsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.BadRequest() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteGraphicsCards(
        [Validate] [FromBody] BulkDeleteGraphicsCardsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
