using PcBuilderBackend.Api.Filters;

namespace PcBuilderBackend.Api.Endpoints.MasterData;

public static class MasterDataEndpoints
{
    public static void MapMasterDataEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/master-data")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithDescription("Browse, Read, Edit, Add and Delete master data entities");

        group.MapManufacturerEndpoints();
        group.MapChipsetEndpoints();
        group.MapCpuSeriesEndpoints();
        group.MapGpuSeriesEndpoints();
        group.MapGpuEndpoints();
        group.MapSocketEndpoints();
    }
}
