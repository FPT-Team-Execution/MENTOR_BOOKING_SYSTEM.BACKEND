using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class MeetingRepository : BaseRepository<Meeting>, IMeetingRepository
{
    public MeetingRepository(IBaseDAO<Meeting> dao) : base(dao)
    {
    }

    public Task<Meeting> GetMeetingByIdAsync(Guid id)
    {
        return _dao.SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Pagination<Meeting>> GetMeetingsPagingAsync(int page, int size)
    {
        return await _dao.GetPagingListAsync(
            page: page,
            size: size
            );
    }
}