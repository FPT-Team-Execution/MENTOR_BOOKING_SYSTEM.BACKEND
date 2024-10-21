using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
    }
}
