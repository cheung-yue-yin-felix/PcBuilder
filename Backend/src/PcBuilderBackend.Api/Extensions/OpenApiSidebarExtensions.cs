using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

// Required for ActionDescriptor mapping

namespace PcBuilderBackend.Api.Extensions;

// Metadata record container
public record GroupSidebarMetadata(string ParentName, string ChildTag);

public static class OpenApiSidebarExtensions
{
    // 1. Fluent Endpoint Group Extension (Works exactly as intended)
    public static RouteGroupBuilder WithSidebarGroup(this RouteGroupBuilder builder, string parentName, string childTag)
    {
        builder.WithTags(childTag);
        builder.WithMetadata(new GroupSidebarMetadata(parentName, childTag));
        return builder;
    }

    // 2. Document Transformer with the proper data mapping paths
    public static OpenApiOptions AutoDiscoverSidebarGroups(this OpenApiOptions options)
    {
        options.AddDocumentTransformer((document, context, _) =>
        {
            // FIX: Dig into apiDesc.ActionDescriptor.EndpointMetadata 
            var discoveredGroupings = context.DescriptionGroups
                .SelectMany(g => g.Items)
                .Where(apiDesc => apiDesc.ActionDescriptor.EndpointMetadata.Count > 0)
                .SelectMany(apiDesc => apiDesc.ActionDescriptor.EndpointMetadata.OfType<GroupSidebarMetadata>())
                .Distinct()
                .GroupBy(m => m.ParentName)
                .ToList();

            if (discoveredGroupings.Count == 0) return Task.CompletedTask;

            document.Extensions ??= new Dictionary<string, IOpenApiExtension>();
            var tagGroupsArray = new JsonArray();

            foreach (var parentGroup in discoveredGroupings)
            {
                var childTagsArray = new JsonArray();
                foreach (var child in parentGroup.Select(c => c.ChildTag).Distinct())
                {
                    childTagsArray.Add(JsonValue.Create(child));
                }

                var tagGroupObject = new JsonObject
                {
                    ["name"] = JsonValue.Create(parentGroup.Key),
                    ["tags"] = childTagsArray
                };
                
                tagGroupsArray.Add(tagGroupObject);
            }

            document.Extensions["x-tagGroups"] = new JsonNodeExtension(tagGroupsArray);
            return Task.CompletedTask;
        });

        return options;
    }
}
