using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(MBSContext context) : base(context)
    {
    }
}