using System.Net;
using MBS.Shared.Models.Google;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Models.Google.GoogleMeeting.Response;
using MBS.Shared.Models.Google.GoogleOAuth.Response;
using MBS.Shared.Utils;



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

        public string GenerateOauthUrl()
        {
            string authenticateUrl = _configuration["Google:Authentication:AuthUrl"]!;
            string calendarScope = _configuration["Google:Authentication:Scope:calendar"]!;
            string profileScope = _configuration["Google:Authentication:Scope:profile"]!;
            string emailScope = _configuration["Google:Authentication:Scope:email"]!;
            string scope = "scope=" + Uri.EscapeDataString($"{calendarScope} {profileScope} {emailScope}");
            string redirectUri = "redirect_uri=" + _configuration["Google:Authentication:CallbackUrl"]!;
            string accessType = "access_type=" + "offline";
            string responseType = "response_type=" + "code";
            string clientId = "client_id=" + _configuration["Google:Authentication:ClientId"]!;
            //string approvalPrompt = "approval_prompt=" + "force";

            //string finalAuthUrl = $"{authenticateUrl}?{scope}&{responseType}&{clientId}&{accessType}&{approvalPrompt}&{redirectUri}";
            string finalAuthUrl = $"{authenticateUrl}?{scope}&{responseType}&{clientId}&{accessType}&{redirectUri}";

            return finalAuthUrl;
        }

        public async Task<GoogleResponse> AuthenticateGoogleUserAsync(HttpContext context)
        {
            // var authenticateResult = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticateResult = await context.AuthenticateAsync(".AspNetCore.Identity.Application");

            if (authenticateResult?.Principal == null) return null;
            // var name = authenticateResult.Principal.FindFirstValue(ClaimTypes.Name);
            // var givenName = authenticateResult.Principal.FindFirstValue(ClaimTypes.GivenName);
            // var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
            // if (email == null) return null;
            // var accessToken = authenticateResult.Properties!.GetTokenValue("access_token");
            // var refreshToken = authenticateResult.Properties!.GetTokenValue("refresh_token");

            var providerKey = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var loginProvider = authenticateResult.Ticket!.Principal.Identity!.AuthenticationType;
            return new GoogleAuthResponse
            {
                ProviderKey = providerKey!,
                LoginProvider = loginProvider!
            };
        }

        public async Task<GoogleResponse> GetTokenGoogleUserAsync(string authenticatedCode, string externalCallbackUri)
        {
            string url = _configuration["Google:Authentication:TokenUrl"]!;
            var requestBody = new Dictionary<string, string>
            {
                { "code", authenticatedCode },
                { "client_id", _configuration["Google:Authentication:ClientId"]! },
                { "client_secret", _configuration["Google:Authentication:ClientSecret"]! },
                //{ "redirect_uri", _configuration["Google:Authentication:CallbackUrl"]! },
                { "redirect_uri", externalCallbackUri },
                { "grant_type", "authorization_code" }
            };

            var response = await WebUtils.PostAsync(url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = WebUtils.HandleResponse<GoogleTokenResponse>(response);
                result.IsSuccess = true;
                return result;
            }

            var errorResult = WebUtils.HandleResponse<GoogleAuthErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;

        }

        public async Task<GoogleResponse> GetProfileGoogleUserAsync(string accessToken)
        {
            string url = "https://www.googleapis.com/oauth2/v3/userinfo";

            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {accessToken}" }
            };
            var response = await WebUtils.GetAsync(url, headers, accessToken);
            if (response.IsSuccessStatusCode)
            {
                var result = WebUtils.HandleResponse<GoogleUserInfoResponse>(response);
                result.IsSuccess = true;
                return result;
            }

            var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;
        }

        // private void SetGoogleAccessToken(string accessToken, DateTime expiredTime)
        // {
        //     _claimService.SetCookieValue("Google.AccessToken", accessToken, expiredTime);
        // }
        
        /// <summary>
        /// Get events from user calendar
        /// </summary>
        /// <returns>List of Google Calendar Event</returns>
        public async Task<GoogleResponse> ListEvents(GetGoogleCalendarEventsRequest getRequest)
        {
            string url = $"https://www.googleapis.com/calendar/v3/calendars/{getRequest.Email}/events";
            var queryParams = new Dictionary<string, string?>
            {
                { "timeMin", getRequest.TimeMin != null ? ConvertUtils.FormatDateTime(getRequest.TimeMin.Value, "yyyy-MM-ddTHH:mm:ssK") : null },
                { "timeMax", getRequest.TimeMax != null ? ConvertUtils.FormatDateTime(getRequest.TimeMax.Value, "yyyy-MM-ddTHH:mm:ssK") : null },
            };
            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {getRequest.AccessToken}" }
            };
            HttpResponseMessage response = await WebUtils.GetAsync(url, headers, getRequest.AccessToken, queryParams);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var successResult = WebUtils.HandleResponse<GetGoogleCalendarEventsResponse>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
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
                    DateTime = ConvertUtils.FormatDateTime(createRequest.Start, "yyyy-MM-ddTHH:mm:ssK"),
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventTime
                {
                    DateTime = ConvertUtils.FormatDateTime(createRequest.End, "yyyy-MM-ddTHH:mm:ssK"),
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
                var successResult = WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
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
                    DateTime = ConvertUtils.FormatDateTime(updateRequest.Start, "yyyy-MM-ddTHH:mm:ssK"),
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventTime
                {
                    DateTime = ConvertUtils.FormatDateTime(updateRequest.End, "yyyy-MM-ddTHH:mm:ssK"),
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
                var successResult = WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
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
                var successResult = WebUtils.HandleResponse<GoogleCalendarEvent>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;

        }
        /// <summary>
        /// format start and end date for Asia_HCM timezone
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        private (DateTime start, DateTime end) GetStartAndEndOfDay(DateTime inputDay)
        {
            // Set the start time to midnight of the given date
            DateTime start = new DateTime(inputDay.Year, inputDay.Month, inputDay.Day, 0, 0, 0, DateTimeKind.Utc);
    
            // Set the end time to one millisecond before midnight of the next day
            DateTime end = new DateTime(start.Year, start.Month, start.Day, 23, 59, 0, DateTimeKind.Utc);

            return (start, end);
        }

        public async Task<GoogleResponse> GetFreeBusyPeriod(FreeBusyParamters request)
        {
            string url = $"https://www.googleapis.com/calendar/v3/freeBusy";
            var headers = new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {request.AccessToken}" }
            };
            var (start, end) = GetStartAndEndOfDay(request.Day);
            var bodyData = new FreeBusyRequest()
            {
                TimeMin = ConvertUtils.FormatDateTime(start, "yyyy-MM-ddTHH:mm:ssK"),
                TimeMax = ConvertUtils.FormatDateTime(end, "yyyy-MM-ddTHH:mm:ssK"),  
                Items = new List<CalendarItem>
                {
                    new CalendarItem
                    {
                        Id = request.Email
                    }
                },
                TimeZone = "Asia/Ho_Chi_Minh"
            };
            HttpResponseMessage response = await WebUtils.PostAsync(url, bodyData, headers, request.AccessToken);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var successResult =  WebUtils.HandleResponse<FreeBusyResponse>(response);
                successResult.IsSuccess = true;
                return successResult;
            }
            //Other response - error
            var errorResult =  WebUtils.HandleResponse<GoogleErrorResponse>(response);
            errorResult.IsSuccess = false;
            return errorResult;
        }

        public async Task<GoogleResponse> CreateMeeting(string accessToken)
        {
            try
            {
                string url = $"https://meet.googleapis.com/v2/spaces";
                var headers = new Dictionary<string, string>
                {
                    { "Accept-Charset", "utf-8" },
                    { "Authorization", $"Bearer {accessToken}" }
                };
                HttpResponseMessage response = await WebUtils.PostAsync(
                    url: url,
                    data: null,
                    headers: headers,
                    accessToken);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var successResult = WebUtils.HandleResponse<GoogleMeetingResponse>(response);
                    successResult.IsSuccess = true;
                    return successResult;
                }

                //Other response - error
                var errorResult = WebUtils.HandleResponse<GoogleErrorResponse>(response);
                errorResult.IsSuccess = false;
                return errorResult;
            }
            catch (Exception e)
            {
                return new GoogleErrorResponse()
                {
                    IsSuccess = false,
                };
            }
        }
    }

}
