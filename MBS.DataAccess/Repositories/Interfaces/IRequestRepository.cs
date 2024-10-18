using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IRequestRepository : IBaseRepository<Request>
{
    Task<Pagination<Request>> GetRequestPaginationAsync(int page, int size, string sortOrder);
    Task<Request?> GetRequestById(Guid id);

}