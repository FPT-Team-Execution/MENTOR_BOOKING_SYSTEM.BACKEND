using System.Text.Json.Serialization;

namespace MBS.Shared.Models.Google.GoogleMeeting.Response;

public class GoogleMeetingResponse : GoogleResponse
{
    [JsonPropertyName("meetingUri")]
    public string MeetingUri { get; set; }
    [JsonPropertyName("meetingCode")]
    public string MeetingCode { get; set; }
    [JsonPropertyName("config")]
    public Config Config { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
public class Config
{
    public string EntryPointAccess { get; set; }
    public string AccessType { get; set; }
}
