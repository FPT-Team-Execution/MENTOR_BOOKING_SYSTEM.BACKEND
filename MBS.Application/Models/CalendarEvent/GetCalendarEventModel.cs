
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.CalendarEvent;

public class CalendarEventPaginationQueryParameters
{
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime? StartTime { get; set; }
    [ RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$")]
    public DateTime? EndTime { get; set; }
    public required string SortBy { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}

public class GetCalendarEventsResponseModel
{
    public IEnumerable<Core.Entities.CalendarEvent> Events { get; set; }
}