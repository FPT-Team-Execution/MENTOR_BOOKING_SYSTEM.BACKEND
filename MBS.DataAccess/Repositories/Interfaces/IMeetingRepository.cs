using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMeetingRepository : IBaseRepository<Meeting>
{
    Task<Meeting> GetMeetingByIdAsync(Guid id);
    Task<Pagination<Meeting>> GetMeetingsPagingAsync(int page, int size);
}