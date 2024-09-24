using MBS.API.ApiDependencyInjections;
using MBS.Application;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations;

namespace MBS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Register data config
            builder.Services.AddDataAccess(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Register application authentication
            builder.Services.AddAppAuthentication(builder.Configuration);

            // Register application services
            builder.Services.AddApplication();

            // Register application swagger gen
            builder.Services.AddAppSwaggerGen();

            var app = builder.Build();
          
            //seed data by automated migration
            using var scope = app.Services.CreateScope();
            AutomatedMigration.MigrateAsync(scope.ServiceProvider).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}