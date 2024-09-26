using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MBS.API.ApiDependencyInjections
{
    public static class AuthenticationDependencyInjection
    {
        //* Do not modified this code -> maybe system is crashed
        public static IServiceCollection AddAppAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidIssuer = configuration["JWT:ValidIssuer"],
                      ValidAudience = configuration["JWT:ValidAudience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                  };
              })
              .AddCookie(options =>
              {
                  options.Cookie.SameSite = SameSiteMode.Lax;
                  options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                  //options.Cookie.Name = "Identity.External";
              })

              .AddGoogle(options =>
              {
                  options.ClientId = configuration.GetValue<string>("GoogleOauthConfig:ClientId"); 
                  options.ClientSecret = configuration.GetValue<string>("GoogleOauthConfig:ClientSecret");
                  //options.CallbackPath = configuration.GetValue<string>("GoogleOauthConfig:CallbackUrl");
                  //options.AccessType = configuration.GetValue<string>("GoogleOauthConfig:AccessType");
                  //options.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                  //options.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                  options.SaveTokens = true;
              });





            return services;
        }
    }
}
