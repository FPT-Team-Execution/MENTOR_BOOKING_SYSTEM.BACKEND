using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.CalendarEvent;

public class CreateCalendarRequestModel
{
    [Required]
    public string Id { get; set; }
    [Required]
    public string Status { get; set; }
    [Required]
    public string HtmlLink { get; set; }
    [Required]
    public DateTime Created { get; set; }
    [Required]
    public DateTime Updated { get; set; }
    [Required]
    public string Summary { get; set; }
    [Required]
    public string ICalUID { get; set; }
    [Required]
    public DateTime Start { get; set; }
    [Required]
    public DateTime End { get; set; }
    [Required]
    public string MentorId { get; set; }
    [Required]
    public string MeetingId { get; set; }

}

public class CreateCalendarResponseModel 
{
    public string CalendarEventId { get; set; }
}