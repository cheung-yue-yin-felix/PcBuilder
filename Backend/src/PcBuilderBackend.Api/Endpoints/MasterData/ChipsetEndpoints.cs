using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkCreateChipsets;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkDeleteChipsets;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.BulkUpdateChipsets;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.CreateChipset;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.DeleteChipset;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.ImportChipsets;
using PcBuilderBackend.Application.MasterData.Chipsets.Commands.UpdateChipset;
using PcBuilderBackend.Application.MasterData.Chipsets.Queries;
using PcBuilderBackend.Application.MasterData.Chipsets.Dto;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class ChipsetEndpoints
{
    public static void MapChipsetEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("chipset")
            .WithSidebarGroup(parentName: "MasterData", childTag: "Chipset")
            .WithDescription("Browse, Read, Edit, Add and Delete Chipsets");

        subgroup.MapGet("/", GetChipsets)
            .Produces<List<ChipsetDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse Chipsets")
            .WithDescription("\n    GET /master-data/chipset");

        subgroup.MapGet("/{id}", GetChipsetById)
            .Produces<ChipsetDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read Chipset by ID")
            .WithDescription("\n    GET /master-data/chipset/{id}");
        
        subgroup.MapPost("/", CreateChipset)
            .Produces<ChipsetDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new Chipset")
            .WithDescription("\n    POST /master-data/chipset");

        subgroup.MapPost("/bulk", BulkCreateChipsets)
            .Produces<List<ChipsetDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple new Chipsets")
            .WithDescription("\n    POST /master-data/chipset/bulk");
        
        subgroup.MapPut("/", UpdateChipset)
            .Produces<ChipsetDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a Chipset")
            .WithDescription("\n    PUT /master-data/chipset");
        
        subgroup.MapPut("/bulk", BulkUpdateChipsets)
            .Produces<List<ChipsetDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple Chipsets")
            .WithDescription("\n    PUT /master-data/chipset/bulk");
        
        subgroup.MapDelete("/{id}", DeleteChipset)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a Chipset")
            .WithDescription("\n    DELETE /master-data/chipset/{id}");
        
        subgroup.MapDelete("/bulk", BulkDeleteChipsets)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple Chipsets")
            .WithDescription("\n    DELETE /master-data/chipset/bulk");

        subgroup.MapPost("/import", ImportChipsets)
            .Produces<List<ChipsetDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import Chipsets from Excel")
            .WithDescription("\n    POST /master-data/chipset/import");
    }

    private static async Task<Ok<List<ChipsetDto>>> GetChipsets(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetChipsetsQuery(), cancellationToken));
    }

    private static async Task<Results<Ok<ChipsetDto>, NotFound>> GetChipsetById(
        [FromServices] ISender sender,
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetChipsetByIdQuery(id), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Created<ChipsetDto>> CreateChipset(
        [Validate] [FromBody] CreateChipsetCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        HttpContext httpContext)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}/{result.Id}", result);
    }

    private static async Task<Results<Ok<ChipsetDto>, NotFound>> UpdateChipset(
        [Validate] [FromBody] UpdateChipsetCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
    )
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteChipset(
        [Validate] [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken
    )
    {
        var success = await sender.Send(new DeleteChipsetCommand(id), cancellationToken);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Created<List<ChipsetDto>>> BulkCreateChipsets(
        [Validate] [FromBody] BulkCreateChipsetsCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}", result);
    }

    private static async Task<Results<Ok<List<ChipsetDto>>, NotFound>> BulkUpdateChipsets(
        [FromBody] BulkUpdateChipsetsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null || result.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    
    private static async Task<Results<NoContent, NotFound>> BulkDeleteChipsets(
        [FromBody] BulkDeleteChipsetsCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var success = await sender.Send(command, cancellationToken);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<ChipsetDto>>> ImportChipsets(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        return TypedResults.Ok(await sender.Send(new ImportChipsetsCommand(fileStream), cancellationToken));
    }
}
