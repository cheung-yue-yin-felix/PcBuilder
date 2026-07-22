using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.CreateManufacturer;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.DeleteManufacturer;
using PcBuilderBackend.Application.Catalog.Manufacturers.Commands.UpdateManufacturer;
using PcBuilderBackend.Application.Catalog.Manufacturers.Dto;
using PcBuilderBackend.Application.Catalog.Manufacturers.Queries;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class ManufacturerEndpoints
{
    public static void MapManufacturerEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("manufacturer")
            .WithSidebarGroup(parentName: "Catalog", childTag: "Manufacturer")
            .WithDescription("Browse, Read, Edit, Add and Delete Manufacturers");

        subgroup.MapGet("/", GetManufacturers)
            .Produces<List<ManufacturerDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse Manufacturers")
            .WithDescription("\n    GET /catalog/manufacturer");

        subgroup.MapGet("/{id}", GetManufacturerById)
            .Produces<ManufacturerDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read Manufacturer by ID")
            .WithDescription("\n    GET /catalog/manufacturer/00000000-0000-0000-0000-000000000000");

        subgroup.MapPut("/", UpdateManufacturer)
            .Produces<ManufacturerDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit Manufacturer")
            .WithDescription("\n    PUT /catalog/manufacturer \n {\n\"id\":\"00000000-0000-0000-0000-000000000000\",\n\"name\":\"ASUS\"\n}");
        
        subgroup.MapPost("/", CreateManufacturer)
            .Produces<ManufacturerDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add Manufacturer")
            .WithDescription("\n    POST /catalog/manufacturer \n {\n\"name\":\"ASUS\"\n}");

        subgroup.MapDelete("/{id}", DeleteManufacturer)
            .Produces<bool>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete Manufacturer")
            .WithDescription("\n    DELETE /catalog/manufacturer/00000000-0000-0000-0000-000000000000");
    }

    private static async Task<Results<Ok<List<ManufacturerDto>>, ProblemHttpResult>> GetManufacturers(
        [FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetManufacturersQuery()));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError); 
        }
    }

    private static async Task<Results<Ok<ManufacturerDto>, ProblemHttpResult>> GetManufacturerById(
        [Validate] [FromRoute] Guid id,
        [FromServices] ISender sender
    )
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetManufacturerByIdQuery(id)));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<Results<Ok<ManufacturerDto>, ProblemHttpResult>> UpdateManufacturer(
        [Validate] [FromBody] UpdateManufacturerCommand command,
        [FromServices] ISender sender
    )
    {
        try
        {
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
    
    private static async Task<Results<Ok<ManufacturerDto>, ProblemHttpResult>> CreateManufacturer(
        [Validate] [FromBody] CreateManufacturerCommand command,
        [FromServices] ISender sender
    )
    {
        try
        {
            return TypedResults.Ok(await sender.Send(command));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<Results<Ok<bool>, ProblemHttpResult>> DeleteManufacturer(
        [Validate] [FromRoute] Guid id,
        [FromServices] ISender sender
    )
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new DeleteManufacturerCommand(id)));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}