using System.Net;
using System.Runtime.Serialization;
using MBS.Shared.Models;
using MBS.Shared.Models.Google;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;


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
        public string FormatDateTime( DateTime dateTime, String format)
        {
            return dateTime.ToString(format);
        }
        /// <summary>
        /// Get events from user calendar
        /// </summary>
        /// <returns>List of Google Calendar Event</returns>
        public async Task<GoogleResponse> ListEvents(GetGoogleCalendarEventsRequest getRequest)
        {
            string url = $"https://www.googleapis.com/calendar/v3/calendars/{getRequest.Email}/events";
            var queryParams = new Dictionary<string, string>
            {
                { "timeMin", FormatDateTime(getRequest.TimeMin, "yyyy-MM-ddTHH:mm:ssK") },
                { "timeMax", FormatDateTime(getRequest.TimeMax, "yyyy-MM-ddTHH:mm:ssK") },
            };
            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {getRequest.AccessToken}" }
            };
            HttpResponseMessage response = await WebUtils.GetAsync(url, headers, getRequest.AccessToken, queryParams);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var successResult =  WebUtils.HandleResponse<GetGoogleCalendarEventsResponse>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult =  WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;
        }
        /// <summary>
        /// Create event in user calendar
        /// </summary>
        /// <returns>Google Calendar Event</returns>
         public async Task<GoogleResponse> InsertEvent(string email, string accessToken, CreateGoogleCalendarEventRequest createRequest)
         {
             string url = $"https://www.googleapis.com/calendar/v3/calendars/{email}/events";
             var headers = new Dictionary<string, string>
             {
                 { "Accept-Charset", "utf-8" },
                 { "Authorization", $"Bearer {accessToken}" }
             };
             //anonymous data object
             var bodyData = new 
             {
               Start = new EventTime
               {
                   DateTime = FormatDateTime(createRequest.Start, "yyyy-MM-ddTHH:mm:ssK"),
                   TimeZone = "Asia/Ho_Chi_Minh"
               },
               End = new EventTime
               {
                   DateTime = FormatDateTime(createRequest.End, "yyyy-MM-ddTHH:mm:ssK"),
                   TimeZone = "Asia/Ho_Chi_Minh"
                }
             };
             var response = await WebUtils.PostAsync(
                 url, 
                 data: bodyData, 
                 headers: headers,
                 token: accessToken
                 );
            
             if (response.StatusCode == HttpStatusCode.OK)
             {
                 var successResult =  WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                 successResult.IsSuccess = true;
                 return successResult;
             }
             //Other response - error
             var errorResult =  WebUtils.HandleResponse<GoogleErrorResponse>(response);
             errorResult.IsSuccess = false;
             return errorResult;

         }
        /// <summary>
        /// update event time in user calendar
        /// </summary>
        /// <returns>Google Calendar Event</returns>
        public async Task<GoogleResponse> UpdateEvent(string email, string eventId, string accessToken, UpdateGoogleCalendarEventRequest updateRequest)
        {
            string url = $"https://www.googleapis.com/calendar/v3/calendars/{email}/events/{eventId}";
            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {accessToken}" }
            };
            //anonymous data object
            var bodyData = new 
            {
                Start = new EventTime
                {
                    DateTime = FormatDateTime(updateRequest.Start, "yyyy-MM-ddTHH:mm:ssK"),
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventTime
                {
                    DateTime = FormatDateTime(updateRequest.End, "yyyy-MM-ddTHH:mm:ssK"),
                    TimeZone = "Asia/Ho_Chi_Minh"
                }
            };
            var response = await WebUtils.PutAsync(
                url, 
                data: bodyData, 
                headers: headers,
                token: accessToken
            );
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var successResult =  WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult =  WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;

        }
        
        /// <summary>
        /// update event time in user calendar
        /// </summary>
        /// <returns>Google Calendar Event</returns>
        public async Task<GoogleResponse> DeleteEvent(string email, string eventId, string accessToken)
        {
            string url = $"https://www.googleapis.com/calendar/v3/calendars/{email}/events/{eventId}";
            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {accessToken}" }
            };
            var response = await WebUtils.DeleteAsync(
                url, 
                headers: headers,
                token: accessToken
            );
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var successResult =  WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult =  WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;

        }
    }
        
}
