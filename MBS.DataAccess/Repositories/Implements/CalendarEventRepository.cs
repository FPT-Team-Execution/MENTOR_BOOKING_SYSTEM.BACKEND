<<<<<<< HEAD
﻿using MBS.Core.Common.Pagination;
=======
using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
>>>>>>> develop
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class CalendarEventRepository : BaseRepository<CalendarEvent>, ICalendarEventRepository
    {
        public CalendarEventRepository(IBaseDAO<CalendarEvent> dao) : base(dao)
        {
        }

        public async Task<CalendarEvent> GetCalendarEventByIdAsync(string id)
        {
            return await _dao.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<CalendarEvent> GetCalendarEventAsync(string id)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: e => e.Id == id,
                include: e => e.Include(x => x.Meeting)
                    );
        }

        public async Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorAsync(string mentorId)
        {
            return await _dao.GetListAsync(e => e.MentorId == mentorId);
        }

        public async Task<Pagination<CalendarEvent>> GetPagedListAsyncByMentorId(int page, int size, string mentorId)
        {
            return await _dao.GetPagingListAsync(
                predicate: s => s.MentorId == mentorId,
                page: page,
                size: size
            );
        }
=======
using Supabase.Storage;

namespace MBS.DataAccess.Repositories.Implements;

public class CalendarEventRepository(IBaseDAO<CalendarEvent> dao) : BaseRepository<CalendarEvent>(dao), ICalendarEventRepository
{
    public async Task<Pagination<CalendarEvent>> GetCalendarEventsByMentorIdPaginationAsync(string mentorId, DateTime? startDate = null, DateTime? endDate = null,
        string sortBy = "asc", int page = 1, int pageSize = 10)
    {
        Expression<Func<CalendarEvent, bool>> predicate = ce => ce.MentorId == mentorId &&
                                                                (startDate == null || ce.Start >= startDate) &&
                                                                (endDate == null || ce.End <= endDate);   
        Func<IQueryable<CalendarEvent>, IOrderedQueryable<CalendarEvent>> orderBy = (sortBy.ToLower() == "asc") ? 
            ce => ce.OrderBy(x => x.Start) : ce => ce.OrderByDescending(x => x.Start);
        return await _dao.GetPagingListAsync(
            predicate: predicate,
            orderBy: orderBy,
            page: page,
            size: pageSize
            );
    }

    public async Task<IEnumerable<CalendarEvent>> GetCalendarEventsByMentorIdAsync(string mentorId, DateTime? startDate, DateTime? endDate)
    {
        Expression<Func<CalendarEvent, bool>> predicate = ce => ce.MentorId == mentorId &&
                                                                (startDate == null || ce.Start >= startDate) &&
                                                                (endDate == null || ce.End <= endDate);   
        Func<IQueryable<CalendarEvent>, IOrderedQueryable<CalendarEvent>> orderBy = ce => ce.OrderBy(x => x.Start);
        return await _dao.GetListAsync(
            predicate: predicate,
            orderBy: orderBy
        );
    }

    public async Task<CalendarEvent?> GetEventByIdAsync(string calendarEventId)
    {
        return await _dao.SingleOrDefaultAsync(
            predicate: x => x.Id == calendarEventId,
            include: x => x.Include(m => m.Meeting).Include(m => m.Mentor)
            );
>>>>>>> develop
    }
}
