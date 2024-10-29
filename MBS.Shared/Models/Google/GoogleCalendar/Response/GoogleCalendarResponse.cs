namespace MBS.Shared.Models.Google.GoogleCalendar.Response;



#region  Success response from Google Calendar 's needed class
public class GetGoogleCalendarEventsResponse : GoogleResponse
{
    public string Kind { get; set; } // "calendar#events"
    public List<GoogleCalendarEvent> Items { get; set; } // List of calendar events
}

public class GoogleCalendarEvent: GoogleResponse
{
    public string Kind { get; set; } // "calendar#event"
    public string Etag { get; set; }
    public string ICalUID { get; set; }
    public string Id { get; set; }
    public string Status { get; set; } // e.g., "confirmed"
    public string HtmlLink { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string Summary { get; set; }
    public EventDateTime Start { get; set; }
    public EventDateTime End { get; set; }
}

public class EventDateTime
{
    public DateTime DateTime { get; set; } //format: yyyy-MM-ddTHH:mm:ssK
    public string TimeZone { get; set; } 
}
#endregion

#region  Error response from Google Calendar 's needed class
public class GoogleErrorResponse : GoogleResponse
{
    public ErrorDetails Error { get; set; }
}

public class ErrorDetails
{
    public List<ErrorInfo> Errors { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } 
}

public class ErrorInfo
{
    public string Domain { get; set; } 
    public string Reason { get; set; } 
    public string Message { get; set; } 
}

#endregion
