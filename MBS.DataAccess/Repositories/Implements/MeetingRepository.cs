<<<<<<< HEAD
using MBS.Core.Common.Pagination;
=======
>>>>>>> develop
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

<<<<<<< HEAD
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
=======
public class MeetingRepository(IBaseDAO<Meeting> dao) : BaseRepository<Meeting>(dao), IMeetingRepository
{
>>>>>>> develop
}