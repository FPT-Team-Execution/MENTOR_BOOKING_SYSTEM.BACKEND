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
using MBS.Shared.Models.Google.Payload;


namespace MBS.Shared.Services.Implements
{
    public class GoogleService : IGoogleService
    {
        private readonly IClaimService _claimService;
        private readonly IConfiguration _configuration;
        public GoogleService(IClaimService claimService, IConfiguration configuration)
        {
            _claimService = claimService;
            _configuration = configuration;
        }
        public async Task<GoogleAuthResponse?> AuthenticateGoogleUserAsync(HttpContext context)
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

            var providerKey = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var loginProvider = authenticateResult.Ticket.Principal.Identity.AuthenticationType;
            return new GoogleAuthResponse
            {
                Name = name,
                GivenName = givenName,
                Email = email,
                GoogleRefreshToken = refreshToken,
                GoogleAccessToken = accessToken,
                ProviderKey = providerKey,
                LoginProvider = loginProvider
            };
        }

        private void SetGoogleAccessToken(string accessToken, DateTime expiredTime)
        {
            _claimService.SetCookieValue("Google.AccessToken", accessToken, expiredTime);
        }
        /// <summary>
        /// Get events from user calendar
        /// </summary>
        /// <returns>List of Google Calendar Event</returns>
        public async Task<List<GoogleCalendarEvent>?> ListEvents(GoogleCalendarEventRequest calendarEventRequest)
        {
            string url = $"https://www.googleapis.com/calendar/v3/calendars/{calendarEventRequest.Email}/events";
            var response = await WebUtils.GetAsync(url, calendarEventRequest.AccessToken, calendarEventRequest.TimeMin, calendarEventRequest.TimeMax);
            if (response == null) return null;
            List<GoogleCalendarEvent> calendarEventList = response.Items;
            
            return calendarEventList;
        }
    }
        
}
