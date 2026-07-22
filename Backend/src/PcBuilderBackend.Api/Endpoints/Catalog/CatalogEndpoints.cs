using PcBuilderBackend.Api.Extensions;
using PcBuilderBackend.Api.Filters;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class CatalogEndpoints
{
    public static void MapCatalogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/catalog")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithDescription("Browse, Read, Edit, Add and Delete products/manufacturers in catalog");
        
        group.MapManufacturerEndpoints();
        group.MapCpuEndpoints();
    }
}