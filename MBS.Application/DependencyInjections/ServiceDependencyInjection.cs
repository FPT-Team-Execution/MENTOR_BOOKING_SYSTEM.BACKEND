using MBS.Application.DependencyInjections;
using MBS.Shared.Services.Implements;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MBS.Application.Services.Interfaces;
using MBS.Application.Services.Implements;
using MBS.DataAccess.Repositories.Interfaces;
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
            services.AddScoped<IGoogleService, GoogleService>();
            services.AddScoped<ICalendarEventService, CalendarEventService>();
            services.AddScoped<ISupabaseService, SupabaseService>();
           

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