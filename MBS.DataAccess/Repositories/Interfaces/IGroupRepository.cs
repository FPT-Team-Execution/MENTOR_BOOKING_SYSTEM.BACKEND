using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder);
}