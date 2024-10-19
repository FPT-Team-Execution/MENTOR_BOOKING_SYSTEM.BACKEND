<<<<<<< HEAD
﻿using MBS.Core.Common.Pagination;
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
=======
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Implements;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface ICalendarEventRepository : IBaseRepository<CalendarEvent>
{
    Task<Pagination<CalendarEvent>> GetCalendarEventsByMentorIdPaginationAsync(
        string mentorId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string sortBy = "asc",
        int page = 1,
        int pageSize = 10);
    Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorIdAsync(
        string mentorId,
        DateTime? startDate,
        DateTime? endDate
        );
    Task<CalendarEvent?> GetEventByIdAsync(string calendarEventId);
}

>>>>>>> develop
