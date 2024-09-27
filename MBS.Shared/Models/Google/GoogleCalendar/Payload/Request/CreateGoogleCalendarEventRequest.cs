namespace MBS.Shared.Models.Google.Payload;

public class CreateGoogleCalendarEventRequest
{
    public EventTime Start { get; set; }
    public EventTime End { get; set; }
}