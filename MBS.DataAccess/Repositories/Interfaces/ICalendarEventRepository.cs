using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface ICalendarEventRepository : IBaseRepository<CalendarEvent>
    {
        Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorAsync(string mentorId);

        Task<CalendarEvent> GetCalendarEventByIdAsync(string id);
        Task<CalendarEvent> GetCalendarEventAsync(string id);
        Task<Pagination<CalendarEvent>> GetPagedListAsyncByMentorId(int page, int size, string mentorId);
    }
}
