using PcBuilderBackend.Api.Filters;

namespace PcBuilderBackend.Api.Endpoints.Catalog;

public static class MapCatalogEndpoints
{
    public static void MapCatalogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("catalog")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithTags("catalog")
            .WithDescription("Browse, Read, Edit, Add and Delete products/manufacturers in catalog");
        
        group.MapManufacturerEndpoints();
    }
}