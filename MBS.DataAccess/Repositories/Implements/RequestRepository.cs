using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(IBaseDAO<Request> requestDao) : base(requestDao)
        {
        }

        public async Task<Pagination<Request>> GetRequestsAsync(int page, int size)
        {
            return await _dao.GetPagingListAsync(page: page, size: size);
        }

        public async Task<Request?> GetRequestByIdAsync(Guid requestId)
        {
            return await _dao.SingleOrDefaultAsync(r => r.Id == requestId);
        }
    }
}
