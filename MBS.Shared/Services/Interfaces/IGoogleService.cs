using MBS.Shared.Models.Google;
using Microsoft.AspNetCore.Http;
using MBS.Shared.Models.Google.Payload;

namespace MBS.Shared.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleAuthResponse?> AuthenticateGoogleUserAsync(HttpContext context);
        Task<List<GoogleCalendarEvent>?> ListEvents(GoogleCalendarEventRequest request);
    }
}
