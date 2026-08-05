using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkCreateCpus;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkDeleteCpus;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpuRamCompats;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.BulkUpdateCpus;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.ImportCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Queries;
using PcBuilderBackend.Application.Common.Dto;
using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class CpuEndpoints
{
    public static void MapCpuEndpoints(this RouteGroupBuilder group)
    {
        var subgroup = group.MapGroup("cpu")
            .WithSidebarGroup("Catalog", "CPU")
            .WithDescription("Browse, Read, Edit, Add and Delete CPUs");

        subgroup.MapGet("/", GetCpus)
            .Produces<PagedResult<CpuListItemDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse CPUs")
            .WithDescription("\n    GET /catalog/cpu");

        subgroup.MapGet("/motherboard/{motherboardId}", GetCpusByMotherboardId)
            .Produces<List<CpuListItemDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse CPUs By Motherboard Id")
            .WithDescription("\n    GET /catalog/cpu/motherboard/00000000-0000-0000-0000-000000000000");

        subgroup.MapGet("/{id}", GetCpuById)
            .Produces<CpuDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Read CPU By ID")
            .WithDescription("\n    GET /catalog/cpu/00000000-0000-0000-0000-000000000000");

        subgroup.MapPut("/", UpdateCpu)
            .Produces<CpuDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit CPU")
            .WithDescription("\n    PUT /catalog/cpu");

        subgroup.MapPut("/bulk", BulkUpdateCpus)
            .Produces<List<CpuDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Edit multiple CPUs")
            .WithDescription("\n    PUT /catalog/cpu/bulk");

        subgroup.MapPost("/", CreateCpu)
            .Produces<CpuDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add CPU")
            .WithDescription("\n    POST /catalog/cpu");

        subgroup.MapPost("/bulk", BulkCreateCpus)
            .Produces<List<CpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Add multiple CPUs")
            .WithDescription("\n    POST /catalog/cpu/bulk");

        subgroup.MapDelete("/{id}", DeleteCpu)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete a CPU")
            .WithDescription("\n    DELETE /catalog/cpu/00000000-0000-0000-0000-000000000000");

        subgroup.MapDelete("/bulk", BulkDeleteCpus)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete multiple CPUs")
            .WithDescription("\n    DELETE /catalog/cpu/bulk");

        subgroup.MapPost("/import", ImportCpus)
            .Produces<List<CpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Accepts<IFormFile>("multipart/form-data")
            .WithSummary("Import CPUs from Excel")
            .WithDescription("\n    POST /catalog/cpu/import");

        var ramCompatGroup = subgroup.MapGroup("{cpuId}/ram-compats")
            .WithDescription("Manage CPU RAM compatibility entries");

        ramCompatGroup.MapGet("/", GetCpuRamCompats)
            .Produces<List<CpuRamCompatDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get CPU RAM compatibility entries")
            .WithDescription("\n    GET /catalog/cpu/00000000-0000-0000-0000-000000000000/ram-compats");

        ramCompatGroup.MapPut("/", UpdateCpuRamCompats)
            .Produces<List<CpuRamCompatDto>>()
            .Produces(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Replace CPU RAM compatibility entries")
            .WithDescription("\n    PUT /catalog/cpu/00000000-0000-0000-0000-000000000000/ram-compats");
    }

    private static async Task<Ok<PagedResult<CpuListItemDto>>> GetCpus(
        [FromServices] ISender sender,
        CancellationToken cancellationToken,
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "manufacturerId")] Guid? manufacturerId,
        [FromQuery(Name = "socketId")] Guid? socketId,
        [FromQuery(Name = "seriesId")] Guid? seriesId,
        [FromQuery(Name = "ddrGeneration")] DdrGeneration? ddrGeneration,
        [FromQuery(Name = "pageIndex")] int pageIndex = 0,
        [FromQuery(Name = "pageSize")] int pageSize = 10)
    {
        var query = new GetCpusQuery(pageIndex, pageSize, name, manufacturerId, socketId, seriesId, ddrGeneration);
        return TypedResults.Ok(await sender.Send(query, cancellationToken));
    }

    private static async Task<Results<Ok<CpuDto>, NotFound>> GetCpuById(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetCpuByIdQuery(id), cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Ok<List<CpuListItemDto>>> GetCpusByMotherboardId(
        [FromRoute] Guid motherboardId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetCpusByMotherboardQuery(motherboardId), cancellationToken));
    }

    private static async Task<Created<CpuDto>> CreateCpu(
        [FromBody] CreateCpuCommand command,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return TypedResults.Created(context.Request.Path, result);
    }

    private static async Task<Results<Ok<CpuDto>, NotFound>> UpdateCpu(
        [FromBody] UpdateCpuCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.NotFound() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> DeleteCpu(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new DeleteCpuCommand(id), cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<CpuRamCompatDto>>> GetCpuRamCompats(
        [FromRoute] Guid cpuId,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        return TypedResults.Ok(await sender.Send(new GetCpuRamCompatsByCpuIdQuery(cpuId), cancellationToken));
    }

    private static async Task<Results<Ok<List<CpuRamCompatDto>>, BadRequest>> UpdateCpuRamCompats(
        [FromRoute] Guid cpuId,
        [FromBody] List<CpuRamCompatDto> ramCompats,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new BulkUpdateCpuRamCompatsCommand(cpuId, ramCompats), cancellationToken);
        return result is null ? TypedResults.BadRequest() : TypedResults.Ok(result);
    }

    private static async Task<Results<Created<List<CpuDto>>, BadRequest>> BulkCreateCpus(
        [FromBody] BulkCreateCpusCommand commands,
        [FromServices] ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(commands, cancellationToken);
        return result.Count == 0 ? TypedResults.BadRequest() : TypedResults.Created(context.Request.Path, result);
    }

    private static async Task<Results<Ok<List<CpuDto>>, BadRequest>> BulkUpdateCpus(
        [FromBody] BulkUpdateCpusCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result is null ? TypedResults.BadRequest() : TypedResults.Ok(result);
    }

    private static async Task<Results<NoContent, NotFound>> BulkDeleteCpus(
        [FromBody] BulkDeleteCpusCommand command,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<Ok<List<CpuDto>>> ImportCpus(
        IFormFile file,
        [FromServices] ISender sender,
        CancellationToken cancellationToken)
    {
        await using var fileStream = file.OpenReadStream();
        var command = new ImportCpusCommand(fileStream);
        return TypedResults.Ok(await sender.Send(command, cancellationToken));
    }
}
