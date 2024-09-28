
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.CalendarEvent;

public class GetCalendarEventsRequestModel
{
    [MaxLength(450), Required]
    public string MentorId { get; set; }
}

public class GetCalendarEventsResponseModel
{
    public IEnumerable<Core.Entities.CalendarEvent> Events { get; set; }
}