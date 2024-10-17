using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Supabase.Storage;

namespace MBS.DataAccess.Repositories.Implements;

public class CalendarEventRepository(IBaseDAO<CalendarEvent> dao) : BaseRepository<CalendarEvent>(dao), ICalendarEventRepository
{
    public async Task<Pagination<CalendarEvent>> GetAllCalendarEventsByMentorIdAsync(string mentorId, CalendarEventQueryParameters parameters)
    {
        Expression<Func<CalendarEvent, bool>> predicate = ce => ce.MentorId == mentorId &&
                                                                (parameters.StartTime == null || ce.Start >= parameters.StartTime ) &&
                                                                (parameters.EndTime == null || ce.End <= parameters.EndTime);   
        Func<IQueryable<CalendarEvent>, IOrderedQueryable<CalendarEvent>> orderBy = (parameters.SortBy == null || parameters.SortBy.ToLower() == "asc") ? 
            ce => ce.OrderBy(x => x.Start) : ce => ce.OrderByDescending(x => x.Start);
        return await _dao.GetPagingListAsync(
            predicate: predicate,
            orderBy: orderBy,
            page: parameters.Page,
            size: parameters.Size
            );
    }

    public async Task<CalendarEvent?> GetByIdAsync(string calendarEventId)
    {
        return await _dao.SingleOrDefaultAsync(
            predicate: x => x.Id == calendarEventId,
            include: x => x.Include(m => m.Meeting)!
            );
    }
}
