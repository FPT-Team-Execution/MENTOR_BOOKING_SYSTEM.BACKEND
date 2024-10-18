using System.Text.Json.Serialization;

namespace MBS.Shared.Models.Google.GoogleCalendar.Request;

public class FreeBusyRequest
{
    [JsonPropertyName("timeMin")]
    public string TimeMin { get; set; }
    [JsonPropertyName("timeMax")]
    public string TimeMax { get; set; }
    [JsonPropertyName("items")]
    public List<CalendarItem> Items { get; set; }
    [JsonPropertyName("timeZone")]
    public string TimeZone { get; set; }
}

public class CalendarItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
}
