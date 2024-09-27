using MBS.Shared.Models;
using MBS.Shared.Models.Google;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;


namespace MBS.Shared.Services.Implements
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly HttpClient _httpClient;
        private readonly IClaimService _claimService;
        private readonly IConfiguration _configuration;
        public GoogleCalendarService(IClaimService claimService, IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _claimService = claimService;
            _configuration = configuration;
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

        private void SetGoogleAccessToken(string accessToken, DateTime expiredTime)
        {
            _claimService.SetCookieValue("Google.AccessToken", accessToken, expiredTime);
        }
        /// <summary>
        /// Get the calendar list from user Google Calendar
        /// </summary>
        /// <returns>List of Google Calendar (using GoogleCalendar class)</returns>
        public async Task<String> GetUserEvents(string accessToken)
        {
            string url = "https://www.googleapis.com/calendar/v3/calendars/datngx.dev@gmail.com/events";
            //url += "?fields=items(id, summary, timeZone)";

            string calendar = await WebUtils.GetResultAsync(url, accessToken);

            return calendar;
        }
    }
        
}
