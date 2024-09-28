using System.ComponentModel.DataAnnotations;
using MBS.Core.Enums;

namespace MBS.Application.Models.CalendarEvent;

public class CreateCalendarRequestModel
{
    [Required]
    public string Id { get; set; }
    [Required, MinLength(200)]
    public EventStatus Status { get; set; }
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
    
    public DateTime Start { get; set; }
    [Required, RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}[+-]\d{2}:\d{2}$")]
    public DateTime End { get; set; }
    [MaxLength(450),Required]
    public string MentorId { get; set; }
    [Required]
    public Guid MeetingId { get; set; }
    
}

public class CreateCalendarResponseModel 
{
    public string CalendarEventId { get; set; }
}