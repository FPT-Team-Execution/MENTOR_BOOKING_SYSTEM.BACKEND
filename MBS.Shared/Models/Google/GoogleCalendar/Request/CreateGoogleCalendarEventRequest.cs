using System.ComponentModel.DataAnnotations;
using MBS.Shared.Models.Google.GoogleCalendar.Response;

namespace MBS.Shared.Models.Google.GoogleCalendar.Request;

public class CreateGoogleCalendarEventRequest
{
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    public string TimeZone { get; set; }
}

public class EventTime
{
    public string DateTime { get; set; }
    public string  TimeZone { get; set; }
}