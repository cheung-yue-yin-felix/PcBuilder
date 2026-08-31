using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkCreateCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkDeleteCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.BulkUpdateCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.CreateCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.DeleteCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.ImportCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Commands.UpdateCpuSeries;
using PcBuilderBackend.Application.MasterData.CpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.CpuSeries.Queries;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class CpuSeriesEndpoints
{
    public static void MapCpuSeriesEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("cpu-series")
            .WithSidebarGroup(parentName: "MasterData", childTag: "CpuSeries")
            .WithDescription("Browse, Read, Edit, Add and Delete CPU Series");
        
        subgroup.MapGet("/", GetCpuSeries)
            .Produces<List<CpuSeriesDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse CPU Series");
        
        subgroup.MapGet("/{cpuSeriesId}", GetCpuSeriesById)
            .Produces<CpuSeriesDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read a CPU Series by ID");
        
        subgroup.MapPut("/", UpdateCpuSeries)
            .Produces<CpuSeriesDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a CPU Series");
        
        subgroup.MapPost("/", CreateCpuSeries)
            .Produces<CpuSeriesDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new CPU Series");
        
        subgroup.MapDelete("/{cpuSeriesId}", DeleteCpuSeries)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a CPU Series");
        
        subgroup.MapPut("/bulk", BulkUpdateCpuSeries)
            .Produces<List<CpuSeriesDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple CPU Series");
        
        subgroup.MapPost("/bulk", BulkCreateCpuSeries)
            .Produces<List<CpuSeriesDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple new CPU Series");
        
        subgroup.MapDelete("/bulk", BulkDeleteCpuSeries)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple CPU Series");

        subgroup.MapPost("/import", ImportCpuSeries)
            .Produces<List<CpuSeriesDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import CPU Series from Excel")
            .WithDescription("\n    POST /master-data/cpu-series/import");
    }

    private static async Task<Ok<List<CpuSeriesDto>>> GetCpuSeries(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetCpuSeriesQuery(), cancellationToken));
    }

    private static async Task<Results<Ok<CpuSeriesDto>, NotFound>> GetCpuSeriesById(
        [FromRoute] Guid cpuSeriesId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCpuSeriesByIdQuery(cpuSeriesId), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    
    private static async Task<Created<CpuSeriesDto>> CreateCpuSeries(
        [Validate] [FromBody] CreateCpuSeriesCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}/{result.Id}", result);
    }

    private static async Task<Results<Ok<CpuSeriesDto>, NotFound>> UpdateCpuSeries(
        [FromBody] UpdateCpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    
    private static async Task<Results<NoContent, NotFound>> DeleteCpuSeries(
        [FromRoute] Guid cpuSeriesId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var success = await sender.Send(new DeleteCpuSeriesCommand(cpuSeriesId), cancellationToken);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Created<List<CpuSeriesDto>>> BulkCreateCpuSeries(
        [Validate] [FromBody] BulkCreateCpuSeriesCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}", result);
    }
    
    private static async Task<Results<Ok<List<CpuSeriesDto>>, NotFound>> BulkUpdateCpuSeries(
        [FromBody] BulkUpdateCpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var results = await sender.Send(command, cancellationToken);
        return results.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(results);
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteCpuSeries(
        [FromBody] BulkDeleteCpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var success = await sender.Send(command, cancellationToken);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<CpuSeriesDto>>> ImportCpuSeries(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        return TypedResults.Ok(await sender.Send(new ImportCpuSeriesCommand(fileStream), cancellationToken));
    }
}
