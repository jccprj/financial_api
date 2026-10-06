using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialAPI.Infrastructure;

/// <summary>
/// Extension methods for Swagger configuration
/// </summary>
public static class SwaggerSecurityExtensions
{
    private const string SchemeName = "Bearer";

    /// <summary>
    /// Adds JWT Bearer authentication support to Swagger (Authorize button and padlock on protected endpoints)
    /// </summary>
    public static void AddSwaggerJwtSecurity(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(SchemeName, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Informe apenas o token JWT (sem o prefixo 'Bearer')."
        });

        options.OperationFilter<AuthorizeOperationFilter>();
    }

    private sealed class AuthorizeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var requiresAuth = metadata.OfType<IAuthorizeData>().Any()
                && !metadata.OfType<IAllowAnonymous>().Any();

            if (!requiresAuth)
                return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference(SchemeName, context.Document)] = new List<string>()
            });
        }
    }
}
