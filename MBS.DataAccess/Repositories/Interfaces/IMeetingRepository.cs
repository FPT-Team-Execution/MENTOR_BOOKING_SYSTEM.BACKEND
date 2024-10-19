<<<<<<< HEAD
using MBS.Core.Common.Pagination;
=======
>>>>>>> develop
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMeetingRepository : IBaseRepository<Meeting>
{
<<<<<<< HEAD
    Task<Meeting> GetMeetingByIdAsync(Guid id);
    Task<Pagination<Meeting>> GetMeetingsPagingAsync(int page, int size);
=======
    
>>>>>>> develop
}