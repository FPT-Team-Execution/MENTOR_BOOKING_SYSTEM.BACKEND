using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Pagination<Project>> GetPagedProject(string search, int page, int pageSize);
}