using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IRequestRepository : IBaseRepository<Request>
    {
        Task<Pagination<Request>> GetRequestsAsync(int page, int size);
        Task<Request?> GetRequestByIdAsync(Guid requestId);
    }
}
