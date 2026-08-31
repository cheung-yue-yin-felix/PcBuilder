using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkCreateGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkDeleteGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.BulkUpdateGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.CreateGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.DeleteGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.ImportGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Commands.UpdateGpuSeries;
using PcBuilderBackend.Application.MasterData.GpuSeries.Dto;
using PcBuilderBackend.Application.MasterData.GpuSeries.Queries;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class GpuSeriesEndpoints
{
    public static void MapGpuSeriesEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("gpu-series")
            .WithSidebarGroup(parentName: "MasterData", childTag: "GpuSeries")
            .WithDescription("Browse, Read, Edit, Add and Delete GPU Series");
        
        subgroup.MapGet("/", GetGpuSeries)
            .Produces<List<GpuSeriesDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse GPU Series");
        
        subgroup.MapGet("/{gpuSeriesId}", GetGpuSeriesById)
            .Produces<GpuSeriesDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read a GPU Series by ID");
        
        subgroup.MapPut("/", UpdateGpuSeries)
            .Produces<GpuSeriesDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a GPU Series");
        
        subgroup.MapPost("/", CreateGpuSeries)
            .Produces<GpuSeriesDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new GPU Series");
        
        subgroup.MapDelete("/{gpuSeriesId}", DeleteGpuSeries)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a GPU Series");
        
        subgroup.MapPost("/bulk", BulkCreateGpuSeries)
            .Produces<List<GpuSeriesDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple new GPU Series");
        
        subgroup.MapPut("/bulk", BulkUpdateGpuSeries)
            .Produces<List<GpuSeriesDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple GPU Series");
        
        subgroup.MapDelete("/bulk", BulkDeleteGpuSeries)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple GPU Series");

        subgroup.MapPost("/import", ImportGpuSeries)
            .Produces<List<GpuSeriesDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import GPU Series from Excel")
            .WithDescription("\n    POST /master-data/gpu-series/import");
    }
    
    private static async Task<Ok<List<GpuSeriesDto>>> GetGpuSeries(
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetGpuSeriesQuery();
        var result = await sender.Send(query, cancellationToken);
        return TypedResults.Ok(result);
    }
    
    private static async Task<Results<Ok<GpuSeriesDto>, NotFound>> GetGpuSeriesById(
        [FromRoute] Guid gpuSeriesId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetGpuSeriesByIdQuery(gpuSeriesId);
        var result = await sender.Send(query, cancellationToken);
        return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
    }

    private static async Task<Created<GpuSeriesDto>> CreateGpuSeries(
        [Validate] [FromBody] CreateGpuSeriesCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}/{result.Id}", result);
    }
    
    private static async Task<Results<Ok<GpuSeriesDto>, NotFound>> UpdateGpuSeries(
        [FromBody] UpdateGpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
    }
    
    private static async Task<Results<NoContent, NotFound>> DeleteGpuSeries(
        [FromRoute] Guid gpuSeriesId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteGpuSeriesCommand(gpuSeriesId);
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
    
    private static async Task<Created<List<GpuSeriesDto>>> BulkCreateGpuSeries(
        [Validate] [FromBody] BulkCreateGpuSeriesCommand command,
        [FromServices] ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}", result);
    }

    private static async Task<Results<Ok<List<GpuSeriesDto>>, NotFound>> BulkUpdateGpuSeries(
        [FromBody] BulkUpdateGpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count == 0 ? TypedResults.NotFound() : TypedResults.Ok(result);
    }
    
    private static async Task<Results<NoContent, NotFound>> BulkDeleteGpuSeries(
        [FromBody] BulkDeleteGpuSeriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<GpuSeriesDto>>> ImportGpuSeries(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        return TypedResults.Ok(await sender.Send(new ImportGpuSeriesCommand(fileStream), cancellationToken));
    }
}
