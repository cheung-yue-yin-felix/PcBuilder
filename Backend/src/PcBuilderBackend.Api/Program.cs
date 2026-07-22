using InfisicalConfiguration;
using PcBuilderBackend.Api.Endpoints.Catalog;
using PcBuilderBackend.Application;
using PcBuilderBackend.Infrastructure;
using Scalar.AspNetCore;
using PcBuilderBackend.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var envSlug = builder.Environment.EnvironmentName switch
{
    "Development" => "dev",
    "Production" => "prod",
    "Staging" => "staging",
    _ => throw new InvalidOperationException()
};

builder.Configuration.AddInfisical(
    new InfisicalConfigBuilder()
        .SetProjectId(builder.Configuration["Infisical:ProjectId"] ?? "")
        .SetEnvironment(envSlug)
        .SetAuth(new InfisicalAuthBuilder()
            .SetUniversalAuth(
                builder.Configuration["Infisical:ClientId"] ?? "",
                builder.Configuration["Infisical:ClientSecret"] ?? "")
            .Build()
        ).Build()
    ).Build();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AutoDiscoverSidebarGroups());
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapCatalogEndpoints();

app.Run();
