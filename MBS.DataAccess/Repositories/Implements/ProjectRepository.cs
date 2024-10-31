using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class ProjectRepository(IBaseDAO<Project> dao) : BaseRepository<Project>(dao), IProjectRepository
{
    public async Task<Pagination<Project>> GetProjectsByMentorId(string mentorId, int page, int size, string sortOrder)
    {
        return await _dao.GetPagingListAsync( 
            predicate: p => p.MentorId == mentorId,
            orderBy: o => (sortOrder.ToLower() == "asc") ? o.OrderBy(x => x.CreatedOn) : o.OrderByDescending(x => x.CreatedOn),
            page: page,
            size: size
            );
    }
}