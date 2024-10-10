using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class MeetingMemberRepository : BaseRepository<MeetingMember>, IMeetingMemberRepository
    {
        public MeetingMemberRepository(IBaseDAO<MeetingMember> dao) : base(dao)
        {
        }

        public async Task<MeetingMember> GetMeetingMemberByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<MeetingMember>> GetMembersInMeetingAsync(Guid meetingId,
        Func<IQueryable<MeetingMember>, IIncludableQueryable<MeetingMember, object>> include = null)
        {
            return await _dao.GetListAsync(
                predicate: a => a.MeetingId == meetingId,
                include: include
            );
        }
        public async Task<MeetingMember> GetMemberInMeetingAsync(Guid meetingId,
    Func<IQueryable<MeetingMember>, IIncludableQueryable<MeetingMember, object>> include = null)
        {
            return await _dao.SingleOrDefaultAsync(
                predicate: a => a.MeetingId == meetingId,
                include: include
            );
        }



    }
}
