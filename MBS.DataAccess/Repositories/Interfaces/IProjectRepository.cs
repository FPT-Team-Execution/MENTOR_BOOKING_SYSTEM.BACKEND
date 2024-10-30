using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Pagination<Project>> GetProjectsByMentorId(string mentorId, int page, int size, string sortOrder);
}