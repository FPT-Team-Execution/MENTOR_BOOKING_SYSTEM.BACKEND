using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMeetingRepository : IBaseRepository<Meeting>
{
    Task<IEnumerable<Meeting>> GetMeetingsByRequests(IEnumerable<Guid> requestIds);
}