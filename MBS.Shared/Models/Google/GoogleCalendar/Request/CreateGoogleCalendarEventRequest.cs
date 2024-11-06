using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MBS.Shared.Models.Google.GoogleCalendar.Response;

namespace MBS.Shared.Models.Google.GoogleCalendar.Request;

public class CreateGoogleCalendarEventRequest
{
    [Required]
    public string Summary { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    public string TimeZone { get; set; }
}


public class EventTime
{
    [JsonPropertyName("dateTime")]
    public string DateTime { get; set; }
    [JsonPropertyName("timeZone")]
    public string  TimeZone { get; set; }
}