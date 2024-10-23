using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class DegreeRepository(IBaseDAO<Degree> dao) : BaseRepository<Degree>(dao), IDegreeRepository
{
    public Task<Pagination<Degree>> GetDegreesByMentorId(string mentorId, int page, int size)
    {
        return dao.GetPagingListAsync(x => x.MentorId == mentorId, page: page, size: size);
    }
}