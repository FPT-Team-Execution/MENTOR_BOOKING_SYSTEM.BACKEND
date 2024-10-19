<<<<<<< HEAD
﻿using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IMeetingMemberRepository : IBaseRepository<MeetingMember> 
    {
        Task<MeetingMember> GetMeetingMemberByIdAsync(Guid id);
        Task<IEnumerable<MeetingMember>> GetMembersInMeetingAsync(Guid meetingId,
        Func<IQueryable<MeetingMember>, IIncludableQueryable<MeetingMember, object>> include = null);

        Task<MeetingMember> GetMemberInMeetingAsync(Guid meetingId,
    Func<IQueryable<MeetingMember>, IIncludableQueryable<MeetingMember, object>> include = null);


    }
}
=======
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMeetingMemberRepository : IBaseRepository<MeetingMember>
{
    public Task<IEnumerable<MeetingMember>> GetMeetingMemberByMeetingIdAsync(Guid id);
    public Task<MeetingMember?> GetMeetingMemberByIdAsync(Guid id);

}
>>>>>>> develop
