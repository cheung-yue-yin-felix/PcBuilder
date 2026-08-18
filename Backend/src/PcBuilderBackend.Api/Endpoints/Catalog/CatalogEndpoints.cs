using PcBuilderBackend.Api.Filters;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/catalog")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithDescription("Browse, Read, Edit, Add and Delete products in catalog");

        group.MapChassisEndpoints();
        group.MapMotherboardEndpoints();
        group.MapCpuEndpoints();
        group.MapMemoryEndpoints();
        group.MapGraphicsCardEndpoints();
        
    }
}