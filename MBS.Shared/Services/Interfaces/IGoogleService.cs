﻿using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using Microsoft.AspNetCore.Http;

namespace MBS.Shared.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleAuthResponse?> AuthenticateGoogleUserAsync(HttpContext context);
        Task<GoogleResponse> ListEvents(GetGoogleCalendarEventsRequest getRequest);
        Task<GoogleResponse> InsertEvent(string email, string accessToken, CreateGoogleCalendarEventRequest createRequest);
        Task<GoogleResponse> UpdateEvent(string email, string eventId, string accessToken, UpdateGoogleCalendarEventRequest createRequest);
        Task<GoogleResponse> DeleteEvent(string email, string eventId, string accessToken);
    }
}
