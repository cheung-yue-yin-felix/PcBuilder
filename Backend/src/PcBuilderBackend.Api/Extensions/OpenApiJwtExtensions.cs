using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace PcBuilderBackend.Api.Extensions;

public static class OpenApiJwtExtensions
{
    public static OpenApiOptions AddJwtBearerSecurity(this OpenApiOptions options)
    {
        options.AddDocumentTransformer((document, _, _) =>
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Bearer authentication. Paste the access token from POST /api/auth/login."
            };
            return Task.CompletedTask;
        });

        options.AddOperationTransformer((operation, context, _) =>
        {
            var authorize = context.Description.ActionDescriptor.EndpointMetadata
                .OfType<IAuthorizeData>()
                .Any();
            if (!authorize)
                return Task.CompletedTask;

            operation.Security ??= [];
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
            });
            return Task.CompletedTask;
        });

        return options;
    }
}
