using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<IEnumerable<Request>> GetRequestByProjectIdAsync(Guid projectId, string? status = null);
    Task<Pagination<Request>> GetRequestByProjectIdPaginationAsync(Guid projectId,int page, int size, string sortOrder, string? requestStatus);
    Task<Pagination<Request>> GetRequestByUserIdPaginationAsync(string userId,int page, int size, string sortOrder, string? requestStatus);
    Task<Pagination<Request>> GetRequestPaginationAsync(int page, int size, string sortOrder);
    Task<Request?> GetRequestById(Guid id);

}