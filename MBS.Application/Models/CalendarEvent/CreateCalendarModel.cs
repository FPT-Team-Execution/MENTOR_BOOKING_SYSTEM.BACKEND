using System.ComponentModel.DataAnnotations;
using MBS.Core.Enums;

namespace MBS.Application.Models.CalendarEvent;

public class CreateCalendarRequestModel
{
    public required string AccessToken { get; set; }
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime Start { get; set; }
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime End { get; set; }
    [MaxLength(450),Required]
    public required string MentorId { get; set; }
    [Required]
    public Guid MeetingId { get; set; }
    
}

public class CreateCalendarResponseModel 
{
    public string CalendarEventId { get; set; }
}