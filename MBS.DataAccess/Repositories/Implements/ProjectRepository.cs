using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(IBaseDAO<Project> dao) : base(dao)
    {
    }

    // public Task<Pagination<Project>> GetProjectByStudetnIdAsync(int page, int size, string studentId)
    // {
    //     return _dao.GetPagingListAsync(
    //         predicate: g => g. == studentId,
    //         include: p => p.Include(x => x.Project),
    //         page: page,
    //         size: size
    //     );
    // }

    public async Task<Project> GetProjectById(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(t => t.Id == id);
    }
}