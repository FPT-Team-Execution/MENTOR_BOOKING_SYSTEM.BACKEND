using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMeetingMemberRepository : IBaseRepository<MeetingMember>
{
    public Task<IEnumerable<MeetingMember>> GetMeetingMemberByMeetingIdAsync(Guid id);
    public Task<MeetingMember?> GetMeetingMemberByIdAsync(Guid id);

}