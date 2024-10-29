using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IGroupRepository : IBaseRepository<Group>
{
        Task<Group> GetGroupByIdAsync(Guid id);
        Task<IEnumerable<Group>> GetGroupByProjectIdAsync(Guid projectId);
        Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder);

    Task<Pagination<Group>> GetPagedListBaseAsync(int page, int size);

}

