using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IProgressRepository : IBaseRepository<Progress>
{
    Task<Pagination<Progress>> GetProgressesAsync(Guid projectId, int pageNumber, int pageSize, string sortOrder);
    Task<Progress?> GetProgressByIdAsync(Guid id);

}