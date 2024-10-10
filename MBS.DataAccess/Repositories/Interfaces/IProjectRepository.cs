using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    //Task<Pagination<Project>> GetProjectByStudetnIdAsync(int page, int size, string studentId);
    
    Task<Project> GetProjectById(Guid id);
}