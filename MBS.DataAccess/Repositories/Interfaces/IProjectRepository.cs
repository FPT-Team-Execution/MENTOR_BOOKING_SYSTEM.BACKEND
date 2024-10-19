<<<<<<< HEAD
using MBS.Core.Common.Pagination;
=======
>>>>>>> develop
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
<<<<<<< HEAD
    //Task<Pagination<Project>> GetProjectByStudetnIdAsync(int page, int size, string studentId);
    
    Task<Project> GetProjectById(Guid id);
=======
    
>>>>>>> develop
}