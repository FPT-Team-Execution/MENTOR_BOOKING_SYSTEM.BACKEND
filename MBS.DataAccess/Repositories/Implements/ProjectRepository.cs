using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class ProjectRepository(IBaseDAO<Project> dao) : BaseRepository<Project>(dao), IProjectRepository
{
    
}