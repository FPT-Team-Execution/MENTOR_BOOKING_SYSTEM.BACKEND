using MBS.API.ApiDependencyInjections;
using MBS.Application.Exceptions;
using MBS.Application.DependencyInjections;
using MBS.DataAccess;
using MBS.DataAccess.Persistents.Configurations;
using MBS.DataAccess.Persistents.Configurations.SeedData;

namespace MBS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy => { policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); });
            });
            // Add services to the container.
            builder.Services.AddControllers();

            // Register data config
            builder.Services.AddDataAccess(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Register application authentication
            builder.Services.AddAppAuthentication();

            // Register supabase
            builder.Services.AddSupabase(builder.Configuration);

            // Register application services
            builder.Services.AddServices();

            // Register application swagger gen
            builder.Services.AddAppSwaggerGen(builder.Configuration);

            // Register application smtp setting
            builder.Services.AddEmailConfiguration(builder.Configuration);

			//Add Auto Mapper
			builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			builder.Services.AddAuthorization();
            var app = builder.Build();
            
            


            //
            //seed data by automated migration
            using var scope = app.Services.CreateScope();
            AutomatedMigration.MigrateAsync(scope.ServiceProvider).GetAwaiter().GetResult();

            using (var scopeDB = app.Services.CreateScope())
            {
                var Dbinit = scopeDB.ServiceProvider.GetRequiredService<SeedMajors>();
                Dbinit.SeedingMajors();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");
			app.Use(async (context, next) =>
			{
				context.Response.Headers.Add("Cross-Origin-Opener-Policy", "same-origin");
				await next();
			});
			app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}