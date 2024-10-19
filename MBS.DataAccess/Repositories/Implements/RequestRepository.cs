<<<<<<< HEAD
﻿using MBS.Core.Common.Pagination;
=======
using MBS.Core.Common.Pagination;
>>>>>>> develop
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
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
=======

namespace MBS.DataAccess.Repositories.Implements;

public class RequestRepository(IBaseDAO<Request> dao) : BaseRepository<Request>(dao), IRequestRepository
{
    public async Task<Pagination<Request>> GetRequestPaginationAsync(int page, int size, string sortOrder)
    {
        return await _dao.GetPagingListAsync(
            orderBy: o => (sortOrder.ToLower() == "asc") ? o.OrderBy(x => x.CreatedOn) : o.OrderByDescending(x => x.CreatedOn),
            page : page,
            size : size);
    }

    public async Task<Request?> GetRequestById(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(
            predicate: r => r.Id == id,
            include: q => q.Include(r => r.Creater).Include(r => r.CalendarEvent)
            );
    }
}
>>>>>>> develop
