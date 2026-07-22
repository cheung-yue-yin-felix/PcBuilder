using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.CreateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.DeleteCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Commands.UpdateCpu;
using PcBuilderBackend.Application.Catalog.Cpus.Dto;
using PcBuilderBackend.Application.Catalog.Cpus.Queries;
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
            .Produces<List<CpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Browse CPUs")
            .WithDescription("\n    GET /catalog/cpu");

        subgroup.MapGet("/{id}", GetCpuById)
            .Produces<CpuDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get CPU By ID")
            .WithDescription("\n    GET /catalog/cpu/00000000-0000-0000-0000-000000000000");

        subgroup.MapGet("/motherboard/{motherboardId}", GetCpusByMotherboardId)
            .Produces<List<CpuDto>>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Get CPUs By Motherboard Id")
            .WithDescription("\n    GET /catalog/cpu/motherboard/00000000-0000-0000-0000-000000000000");

        subgroup.MapPost("/", CreateCpu)
            .Produces<CpuDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Create CPU")
            .WithDescription("\n    POST /catalog/cpu");

        subgroup.MapPut("/", UpdateCpu)
            .Produces<CpuDto>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Update CPU")
            .WithDescription("\n    PUT /catalog/cpu");

        subgroup.MapDelete("/{id}", DeleteCpu)
            .Produces<bool>()
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithSummary("Delete CPU")
            .WithDescription("\n    DELETE /catalog/cpu/00000000-0000-0000-0000-000000000000");
    }
    
    private static async Task<Results<Ok<List<CpuDto>>, ProblemHttpResult>> GetCpus(
        [FromQuery(Name = "name")] string? name,
        [FromQuery(Name = "manufacturerId")] Guid? manufacturerId,
        [FromQuery(Name = "socketId")] Guid? socketId,
        [FromQuery(Name = "seriesId")] Guid? seriesId,
        [FromQuery(Name = "ddrGeneration")] DdrGeneration? ddrGeneration,
        [FromServices] ISender sender)
    {
        try
        {
            var query = new GetCpusQuery(name, manufacturerId, socketId, seriesId, ddrGeneration);
            return TypedResults.Ok(await sender.Send(query));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError); 
        }
    }

    private static async Task<Results<Ok<CpuDto>, ProblemHttpResult>> GetCpuById(
        [FromRoute] Guid id,
        [FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetCpuByIdQuery(id)));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<Results<Ok<List<CpuDto>>, ProblemHttpResult>> GetCpusByMotherboardId(
        [FromRoute] Guid motherboardId,
        [FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new GetCpusByMotherboardQuery(motherboardId)));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<Results<Ok<CpuDto>, ProblemHttpResult>> CreateCpu(
        [FromBody] CreateCpuCommand command,
        [FromServices] ISender sender)
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
    
    private static async Task<Results<Ok<CpuDto>, ProblemHttpResult>> UpdateCpu(
        [FromBody] UpdateCpuCommand command,
        [FromServices] ISender sender)
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

    private static async Task<Results<Ok<bool>, ProblemHttpResult>> DeleteCpu(
        [FromRoute] Guid id,
        [FromServices] ISender sender)
    {
        try
        {
            return TypedResults.Ok(await sender.Send(new DeleteCpuCommand(id)));
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }
}