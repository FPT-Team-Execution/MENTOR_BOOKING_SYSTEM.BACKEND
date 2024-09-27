using System.ComponentModel.DataAnnotations;

namespace MBS.Shared.Models.Google.GoogleCalendar.Request;

public class UpdateGoogleCalendarEventRequest
{
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    public string TimeZone { get; set; }
}