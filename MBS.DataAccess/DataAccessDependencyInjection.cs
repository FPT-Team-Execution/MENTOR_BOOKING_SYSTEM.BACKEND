﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.DAO.Implements;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess
{
    public static class DataAccessDependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            services.AddIdentity();
            services.AddDao();
            services.AddRepositories();

            return services;
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MBSContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MBS"),
                    opt => opt.MigrationsAssembly(typeof(MBSContext).Assembly.FullName)));
        }

        private static void AddDao(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseDAO<>), typeof(BaseDAO<>));
        }
        private static void AddRepositories(this IServiceCollection services)
        {
            //TODO: remove old unitOfWork
            services.AddScoped<DAO.IUnitOfWork, DAO.UnitOfWork>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IMentorRepository, MentorRepository>();
            services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IFeedbackRepository, FeedBackRepository>();
            services.AddScoped<IMeetingMemberRepository, MeetingMemberRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();

        }

        private static void AddIdentity(this IServiceCollection services)
        {
            // Register the Identity services with default configuration
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MBSContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}