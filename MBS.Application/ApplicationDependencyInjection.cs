using MBS.Application.Common.Email;
using MBS.Shared.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MBS.Application.Services.Interfaces;
using MBS.Application.Services.Implements;
using MBS.Shared.Services.Interfaces;


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
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITemplateService, TemplateService>();
            services.AddScoped<IGoogleAuthenticationService, GoogleAuthenticationService>();

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