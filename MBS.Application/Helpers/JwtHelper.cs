using MBS.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MBS.Application.Helpers;

public static class JwtHelper
{
    public static string GenerateJwtAccessTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        var userRoles = userManager.GetRolesAsync(user).GetAwaiter().GetResult();

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        var tokenObject = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(60),
            claims: authClaims,
            signingCredentials: signingCredentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return accessToken;
    }

    public static string GenerateJwtRefreshTokenAsync(ApplicationUser user, IConfiguration configuration)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
        var signingCredentials = new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

        var tokenObject = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddDays(3),
            claims: authClaims,
            signingCredentials: signingCredentials
        );

        var refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenObject);

        return refreshToken;
    }

    public static ClaimsPrincipal? GetPrincipalFromJwtToken(string token, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            // Token validation failed
            return null;
        }
    }
}