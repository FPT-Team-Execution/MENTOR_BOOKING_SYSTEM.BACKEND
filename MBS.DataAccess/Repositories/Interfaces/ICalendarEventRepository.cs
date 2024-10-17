using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Implements;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface ICalendarEventRepository : IBaseRepository<CalendarEvent>
{
    Task<Pagination<CalendarEvent>> GetAllCalendarEventsByMentorIdAsync(string mentorId,
        CalendarEventQueryParameters parameters);
    Task<CalendarEvent?> GetByIdAsync(string calendarEventId);
}
public class CalendarEventQueryParameters
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}