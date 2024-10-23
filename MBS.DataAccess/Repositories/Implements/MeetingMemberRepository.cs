using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class MeetingMemberRepository(IBaseDAO<MeetingMember> dao) : BaseRepository<MeetingMember>(dao), IMeetingMemberRepository
{
    public async Task<IEnumerable<MeetingMember>> GetMeetingMemberByMeetingIdAsync(Guid id)
    {
        return await _dao.GetListAsync(
            predicate: mm => mm.MeetingId == id,
            include: q => q.Include(mm => mm.Student)
            );
    }

    public async Task<MeetingMember?> GetMeetingMemberByIdAsync(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(
            predicate: mm => mm.MeetingId == id,
            include: q => q.Include(mm => mm.Meeting)
        );
    }
}

