using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleOAuth.Response;
using Microsoft.AspNetCore.Http;

namespace MBS.Shared.Services.Interfaces
{
    public interface IGoogleService
    {
        String GenerateOauthUrl();
        //* Google Auth
        Task<GoogleResponse> AuthenticateGoogleUserAsync(HttpContext context);
        Task<GoogleResponse> GetTokenGoogleUserAsync(string authenticatedCode, string externalCallbackUri);
        Task<GoogleResponse> GetProfileGoogleUserAsync(string accessToken);

        //* Google Calendar
        Task<GoogleResponse> ListEvents(GetGoogleCalendarEventsRequest getRequest);
        Task<GoogleResponse> InsertEvent(string email, string accessToken, CreateGoogleCalendarEventRequest createRequest);
        Task<GoogleResponse> UpdateEvent(string email, string eventId, string accessToken, UpdateGoogleCalendarEventRequest updateRequest);
        Task<GoogleResponse> DeleteEvent(string email, string eventId, string accessToken);
        Task<GoogleResponse> GetFreeBusyPeriod(FreeBusyParamters request);

    }
}
