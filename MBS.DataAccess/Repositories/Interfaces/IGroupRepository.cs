using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> GetGroupByIdAsync(Guid id);
        Task<IEnumerable<Group>> GetGroupsByProjectIdAsync(Guid projectId);

        Task<Group> GetGroupByProjectAndStudentIdAsync(Guid projectId, string studentId);
    }
}
