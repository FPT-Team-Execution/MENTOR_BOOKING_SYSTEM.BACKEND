namespace MBS.Shared.Models.Google.Payload;

public class GoogleCalendarEventRequest
{
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public DateTime TimeMax { get; set; }
    public DateTime TimeMin { get; set; }
}