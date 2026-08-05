using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;
using PcBuilderBackend.Application.Catalog.Memories.Commands.BulkCreateMemories;
using PcBuilderBackend.Application.Catalog.Memories.Commands.BulkDeleteMemories;
using PcBuilderBackend.Application.Catalog.Memories.Commands.BulkUpdateMemories;
using PcBuilderBackend.Application.Catalog.Memories.Commands.CreateMemory;
using PcBuilderBackend.Application.Catalog.Memories.Commands.DeleteMemory;
using PcBuilderBackend.Application.Catalog.Memories.Commands.UpdateMemory;
using PcBuilderBackend.Application.Catalog.Memories.Commands.ImportMemories;
using PcBuilderBackend.Application.Catalog.Memories.Dto;
using PcBuilderBackend.Application.Catalog.Memories.Queries;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class MemoryEndpoints
{
    public static void MapMemoryEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("ram")
            .WithSidebarGroup("Catalog", "RAM")
            .WithDescription("Browse, Read, Edit, Add and Delete RAM Modules");

        subgroup.MapGet("/", GetMemories)
            .Produces<List<RamDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse RAMs")
            .WithDescription("\n    GET /catalog/ram");

        subgroup.MapGet("/cpu/{cpuId}", GetMemoriesByCpuId)
            .Produces<List<RamDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse RAMs By CPU Id")
            .WithDescription("\n    GET /catalog/ram/cpu/00000000-0000-0000-0000-000000000000");

        subgroup.MapGet("/{id}", GetMemoryById)
            .Produces<RamDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read a RAM By ID")
            .WithDescription("\n    GET /catalog/ram/00000000-0000-0000-0000-000000000000");
        
        subgroup.MapGet("/cpu/{cpuId}/motherboard/{motherboardId}", GetMemoriesByCpuIdAndMotherboardId)
            .Produces<List<RamDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse RAMs By CPU Id and Motherboard Id")
            .WithDescription("\n    GET /catalog/ram/cpu/00000000-0000-0000-0000-000000000000/motherboard/00000000-0000-0000-0000-000000000000");

        subgroup.MapPut("/", UpdateMemory)
            .Produces<RamDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit a RAM")
            .WithDescription("\n    PUT /catalog/ram");
        
        subgroup.MapPut("/bulk", BulkUpdateMemories)
            .Produces<List<RamDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple RAMs")
            .WithDescription("\n    PUT /catalog/ram/bulk");

        subgroup.MapPost("/", CreateMemory)
            .Produces<RamDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add a new RAM")
            .WithDescription("\n    POST /catalog/ram");
        
        subgroup.MapPost("/bulk", BulkCreateMemories)
            .Produces<List<RamDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple new RAMs")
            .WithDescription("\n    POST /catalog/ram/bulk");

        subgroup.MapDelete("/{id}", DeleteMemory)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a RAM")
            .WithDescription("\n    DELETE /catalog/ram/00000000-0000-0000-0000-000000000000");
        
        subgroup.MapDelete("/bulk", BulkDeleteMemories)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple RAMs")
            .WithDescription("\n    DELETE /catalog/ram/bulk");
        
        subgroup.MapPost("/import", ImportMemories)
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<List<RamDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Import RAMs from Excel")
            .WithDescription("\n    POST /catalog/ram/import");
    }

    private static async Task<Ok<List<RamDto>>> GetMemories(
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "manufacturerId")] Guid? manufacturerId,
        [FromQuery(Name = "color")] string? color,
        [FromQuery(Name = "ddrGeneration")] DdrGeneration? ddrGeneration,
        [FromQuery(Name = "ramFormFactor")] RamFormFactor? ramFormFactor,
        [FromQuery(Name = "ramRank")] RamRank? ramRank,
        [FromQuery(Name = "memorySizePerStickGb")] int? memorySizePerStickGb,
        [FromQuery(Name = "totalMemorySizeGb")] int? totalMemorySizeGb,
        [FromQuery(Name = "modulesCount")] int? modulesCount,
        [FromQuery(Name = "maxMemorySpeedMts")] int? maxMemorySpeedMts,
        [FromQuery(Name = "minHeightMm")] decimal? minHeightMm,
        [FromQuery(Name = "maxHeightMm")] decimal? maxHeightMm,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetMemoriesQuery
        {
            Name = name,
            ManufacturerId = manufacturerId,
            Color = color,
            DdrGeneration = ddrGeneration,
            RamFormFactor = ramFormFactor,
            RamRank = ramRank,
            MemorySizePerStickGb = memorySizePerStickGb,
            TotalMemorySizeGb = totalMemorySizeGb,
            ModulesCount = modulesCount,
            MaxMemorySpeedMts = maxMemorySpeedMts,
            MinHeightMm = minHeightMm,
            MaxHeightMm = maxHeightMm
        };

        return TypedResults.Ok(await sender.Send(query, cancellationToken));
    }

    private static async Task<Results<Ok<RamDto>, NotFound>> GetMemoryById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMemoryByIdQuery(id), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Ok<List<RamDto>>> GetMemoriesByCpuId(
        [FromRoute] Guid cpuId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetMemoriesByCpuIdQuery(cpuId), cancellationToken));
    }

    private static async Task<Ok<List<RamDto>>> GetMemoriesByCpuIdAndMotherboardId(
        [FromRoute] Guid cpuId,
        [FromRoute] Guid motherboardId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(
            await sender.Send(new GetMemoriesByCpuIdAndMotherboardIdQuery(cpuId, motherboardId), cancellationToken));
    }

    private static async Task<Ok<RamDto>> CreateMemory(
        [Validate] [FromBody] CreateMemoryCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(command, cancellationToken));
    }

    private static async Task<Results<Ok<RamDto>, NotFound>> UpdateMemory(
        [Validate] [FromBody] UpdateMemoryCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteMemory(
        [Validate] [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var success = await sender.Send(new DeleteMemoryCommand(id), cancellationToken);
        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Created<List<RamDto>>> BulkCreateMemories(
        [FromBody] BulkCreateMemoriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        HttpContext httpContext)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created($"{httpContext.Request.Path}", result);
    }

    private static async Task<Results<Ok<List<RamDto>>, BadRequest>> BulkUpdateMemories(
        [FromBody] BulkUpdateMemoriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.Count == 0 ? TypedResults.BadRequest() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteMemories(
        [FromBody] BulkDeleteMemoriesCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<RamDto>>> ImportMemories(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();
        var result = await sender.Send(new ImportMemoriesCommand(stream), cancellationToken);
        return TypedResults.Ok(result);
    }
}

