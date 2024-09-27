using System.ComponentModel.DataAnnotations;

namespace MBS.Shared.Models.Google.GoogleCalendar.Request;

public class GetGoogleCalendarEventsRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public DateTime TimeMax { get; set; }
    [Required]
    public DateTime TimeMin { get; set; }
}