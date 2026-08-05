using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkCreateGpus;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkDeleteGpus;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.BulkUpdateGpus;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.CreateGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.DeleteGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Commands.UpdateGpu;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.MasterData.Gpus.Queries;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class GpuEndpoints
{
    public static void MapGpuEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("gpu")
            .WithSidebarGroup("MasterData", "GPU")
            .WithDescription("Browse, Read, Edit, Add and Delete GPU chips (reference data)");

        subgroup.MapGet("/", GetGpus)
            .Produces<PagedResult<GpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse GPUs")
            .WithDescription("\n    GET /master-data/gpu?manufacturerId={manufacturerId}&gpuSeriesId={gpuSeriesId}&name={name}");

        subgroup.MapGet("/{id:guid}", GetGpuById)
            .Produces<GpuDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read a GPU by ID")
            .WithDescription("\n    GET /master-data/gpu/{id}");

        subgroup.MapPut("/", UpdateGpu)
            .Produces<GpuDto>()
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a GPU")
            .WithDescription("\n    PUT /master-data/gpu");

        subgroup.MapPut("/bulk", BulkUpdateGpus)
            .Produces<List<GpuDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple GPUs")
            .WithDescription("\n    PUT /master-data/gpu/bulk");

        subgroup.MapPost("/", CreateGpu)
            .Produces<GpuDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new GPU")
            .WithDescription("\n    POST /master-data/gpu");

        subgroup.MapPost("/bulk", BulkCreateGpus)
            .Produces<List<GpuDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple GPUs")
            .WithDescription("\n    POST /master-data/gpu/bulk");

        subgroup.MapDelete("/{id:guid}", DeleteGpu)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a GPU")
            .WithDescription("\n    DELETE /master-data/gpu/{id}");

        subgroup.MapDelete("/bulk", BulkDeleteGpus)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple GPUs")
            .WithDescription("\n    DELETE /master-data/gpu/bulk");

        subgroup.MapPost("/import", ImportGpus)
            .Produces<List<GpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import GPUs from Excel")
            .WithDescription("\n    POST /master-data/gpu/import");
    }

    private static async Task<Ok<PagedResult<GpuDto>>> GetGpus(
        [FromQuery] Guid? manufacturerId,
        [FromQuery] Guid? gpuSeriesId,
        [FromQuery] string? name,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery(Name = "pageIndex")] int pageIndex = 0,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        var query = new GetGpusQuery(pageIndex, pageSize, name, manufacturerId, gpuSeriesId);
        return TypedResults.Ok(await sender.Send(query, cancellationToken));
    }

    private static async Task<Results<Ok<GpuDto>, NotFound>> GetGpuById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetGpuByIdQuery(id), cancellationToken);
        return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
    }

    private static async Task<Created<GpuDto>> CreateGpu(
        [FromBody] CreateGpuCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        HttpContext httpContext)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}/{result.Id}", result);
    }

    private static async Task<Results<Ok<GpuDto>, BadRequest>> UpdateGpu(
        [FromBody] UpdateGpuCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is not null ? TypedResults.Ok(result) : TypedResults.BadRequest();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteGpu(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteGpuCommand(id), cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<GpuDto>>> ImportGpus(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        return TypedResults.Ok(await sender.Send(new ImportGpusCommand(fileStream), cancellationToken));
    }

    private static async Task<Results<Created<List<GpuDto>>, BadRequest>> BulkCreateGpus(
        [FromBody] BulkCreateGpusCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        HttpContext httpContext)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count > 0
            ? TypedResults.Created($"{httpContext.Request.Path}", result)
            : TypedResults.BadRequest();
    }

    private static async Task<Results<Ok<List<GpuDto>>, BadRequest>> BulkUpdateGpus(
        [FromBody] BulkUpdateGpusCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count > 0 ? TypedResults.Ok(result) : TypedResults.BadRequest();
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteGpus(
        [FromBody] BulkDeleteGpusCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
