﻿using MBS.Shared.Services.Implements;
using MBS.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;


namespace MBS.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();
            //services.RegisterAutoMapper();

            return services;
        }
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();
            // services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            // services.AddScoped<ITodoListService, TodoListService>();
            // services.AddScoped<ITodoItemService, TodoItemService>();
            // services.AddScoped<IUserService, UserService>();
            // services.AddScoped<IClaimService, ClaimService>();
            // services.AddScoped<ITemplateService, TemplateService>();

            // Kiểm tra môi trường để đăng ký dịch vụ email
            //services.AddScoped<IEmailService, EmailService>();
        }
        //private static void RegisterAutoMapper(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(typeof(IMappingProfilesMarker));
        //}

    }
}
