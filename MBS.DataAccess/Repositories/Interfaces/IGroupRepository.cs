using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IGroupRepository : IBaseRepository<Group>
{
<<<<<<< HEAD
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> GetGroupByIdAsync(Guid id);
        Task<IEnumerable<Group>> GetGroupByProjectIdAsync(Guid projectId);
    }
}
=======
    Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder);
}
>>>>>>> parent of 4cb5763 (merge query to test api with data)
