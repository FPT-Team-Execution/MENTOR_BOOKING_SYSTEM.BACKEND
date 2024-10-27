using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace MBS.DataAccess.Repositories.Implements;

public class ProjectRepository(IBaseDAO<Project> dao) : BaseRepository<Project>(dao), IProjectRepository
{
    public async Task<Pagination<Project>> GetPagedProject(string search, int page, int pageSize)
    {
        return await _dao.GetPagingListAsync(
            predicate: x => x.Title.ToLower().Contains(search.ToLower()),
            page: page,
            size: pageSize);
    }
}