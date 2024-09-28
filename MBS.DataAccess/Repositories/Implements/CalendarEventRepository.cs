using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class CalendarEventRepository : BaseRepository<CalendarEvent>,ICalendarEventRepository
{
    public CalendarEventRepository(MBSContext context) : base(context)
    {
    }
}