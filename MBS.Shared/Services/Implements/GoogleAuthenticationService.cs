using MBS.Shared.Models.Google;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Xml.Linq;


namespace MBS.Shared.Services.Implements
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        private readonly IClaimService _claimService;
        public GoogleAuthenticationService(IClaimService claimService)
        {
            _claimService = claimService;
        }
        public async Task<GoogleAuthResponse> AuthenticateGoogleUser(HttpContext context)
        {
            //var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticateResult = await context.AuthenticateAsync("Identity.External");

            if (authenticateResult.Principal == null) return null;
            var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);
            var givenName = authenticateResult.Principal.FindFirstValue(ClaimTypes.GivenName);
            var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null) return null;
            var accessToken = authenticateResult.Properties.GetTokenValue("access_token");
            var refreshToken = authenticateResult.Properties.GetTokenValue("refresh_token");
            var expireAt = DateTime.Parse(authenticateResult.Properties.GetTokenValue("expires_at"));
            //Save to cookie
            SetGoogleAccessToken(accessToken, expireAt);

            return new GoogleAuthResponse
            {
                Name = name,
                GivenName = givenName,
                Email = email,
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };
        }

        //public GoogleToken? GetGooogleToken(HttpContext context)
        //{
        //    var googleToken = new GoogleToken();
        //    //check in cookie
        //    var accessToken = _claimService.GetCookieValue("Google.AccessToken");
        //    var accessTokenExpiredTime = _claimService.GetCookieExpiredTime("Google.AccessToken");
        //    if (String.IsNullOrEmpty(accessToken)) return null;
        //    return new GoogleToken
        //    {
        //       Access_token = accessToken,
        //       Expires_in = DateTime.Parse(accessTokenExpiredTime),
        //    };
        //}
        private void SetGoogleAccessToken(string accessToken, DateTime expiredTime)
        {
            _claimService.SetCookieValue("Google.AccessToken", accessToken, expiredTime);
        }
    }
}
