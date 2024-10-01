using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace MBS.Application.DependencyInjections
{
    public static class SwaggerDependencyInjection
    {
        public static IServiceCollection AddAppSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                var googleOAuthConfig = configuration.GetSection("GoogleOauthConfig");
                var clientId = googleOAuthConfig["ClientId"];
                var clientSecret = googleOAuthConfig["ClientSecret"];
                var authorizationUrl = googleOAuthConfig["AuthUri"];
                var tokenUrl = googleOAuthConfig["TokenUri"];
                var scopes = googleOAuthConfig.GetSection("Scopes").Get<Dictionary<string, string>>();

                // OAuth2 configuration
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationUrl!),
                            TokenUrl = new Uri(tokenUrl!),
                            Scopes = scopes
                        }
                    }
                });

                // Bearer token configuration
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                // Security requirements for OAuth2 and Bearer
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2" // OAuth2 reference
                            }
                        },
                        scopes.Keys.ToList()
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // Bearer reference
                            }
                        },
                        new List<string>() // Empty list for Bearer
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            return services;
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if any AuthorizeAttribute exists on either the class or method
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>().Any() ||
                            context.MethodInfo.GetCustomAttributes(true)
                                .OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                // Apply security requirements for both OAuth2 and Bearer token
                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "oauth2"
                                }
                            },
                            new[] { "openid", "profile", "email" }
                        }
                    },
                    new OpenApiSecurityRequirement()
                    {
                        {
                            new OpenApiSecurityScheme()
                            {
                                Reference = new OpenApiReference()
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>() // Empty list for Bearer token
                        }
                    }
                };
            }
        }
    }
}
