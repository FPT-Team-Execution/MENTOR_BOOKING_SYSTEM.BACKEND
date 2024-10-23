
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.CalendarEvent;

public class CalendarEventPaginationQueryParameters
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}

public class GetCalendarEventsResponseModel
{
    public IEnumerable<Core.Entities.CalendarEvent> Events { get; set; }
}