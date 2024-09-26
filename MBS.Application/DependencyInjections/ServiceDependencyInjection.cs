using MBS.Shared.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MBS.Application.Services.Interfaces;
using MBS.Application.Services.Implements;
using MBS.Shared.Common.Email;
using MBS.Shared.Services.Interfaces;


namespace MBS.Application
{
    public static class ServiceDependencyInjection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IMentorService, MentorService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITemplateService, TemplateService>();
            // services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            // services.AddScoped<ITodoListService, TodoListService>();
            // services.AddScoped<ITodoItemService, TodoItemService>();
            // services.AddScoped<IAuthService, AuthService>();
            // services.AddScoped<IClaimService, ClaimService>();
            // services.AddScoped<ITemplateService, TemplateService>();

            // Kiểm tra môi trường để đăng ký dịch vụ email
            //services.AddScoped<IEmailService, EmailService>();
        }

        //private static void RegisterAutoMapper(this IServiceCollection services)
        //{
        //    services.AddAutoMapper(typeof(IMappingProfilesMarker));
        //}

        public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration.GetSection("SmtpSettings").Get<SmtpSettings>()!);
        }
    }
}