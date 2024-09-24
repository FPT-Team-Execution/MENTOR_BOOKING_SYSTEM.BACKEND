using MBS.Application.Services.Implements;
using MBS.Application.Services.Interfaces;

namespace MBS.API.ApiDependencyInjections;

public static class ServicesDependencyInjection
{
    public static WebApplicationBuilder AddAppServices(this WebApplicationBuilder builder)
    {
        // Register user service
        builder.Services.AddScoped<IUserService, UserService>();

        return builder;
    }
}