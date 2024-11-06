using System.Linq.Expressions;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class RequestRepository(IBaseDAO<Request> dao) : BaseRepository<Request>(dao), IRequestRepository
{
    public async Task<IEnumerable<Request>> GetRequestByProjectIdAsync(Guid projectId, string? status)
    {
        
        RequestStatusEnum? statusEnum = null;
        if (!string.IsNullOrEmpty(status))
        {
            statusEnum = Enum.Parse<RequestStatusEnum>(status);
        }

        Expression<Func<Request, bool>> filter = x => x.ProjectId == projectId && 
                                                      (statusEnum == null || x.Status == statusEnum);

        return await _dao.GetListAsync(
            predicate:  filter,
            include: q => q.Include(x => x.Project));
    }
    public async Task<Pagination<Request>> GetRequestByProjectIdPaginationAsync(Guid projectId, int page, int size, string sortOrder, string? requestStatus = null)
    {
        RequestStatusEnum? statusEnum = null;
        if (!string.IsNullOrEmpty(requestStatus))
        {
            statusEnum = Enum.Parse<RequestStatusEnum>(requestStatus, true);
        }

        Expression<Func<Request, bool>> filter = x =>
            x.ProjectId == projectId &&
            (statusEnum == null || x.Status == statusEnum);

        return await _dao.GetPagingListAsync(
            predicate: filter,
            orderBy: o => (sortOrder.ToLower() == "asc") ? o.OrderBy(x => x.CreatedOn) : o.OrderByDescending(x => x.CreatedOn),
            page : page,
            size : size);
    }

    public async Task<Pagination<Request>> GetRequestByUserIdPaginationAsync(string userId, int page, int size, string sortOrder, string? requestStatus)
    {
        Expression<Func<Request, bool>> filter = x => x.CreaterId == userId 
                                                      && (!string.IsNullOrEmpty(requestStatus) && x.Status == Enum.Parse<RequestStatusEnum>(requestStatus, true)); ;
        return await _dao.GetPagingListAsync(
            predicate: filter,
            orderBy: o => (sortOrder.ToLower() == "asc") ? o.OrderBy(x => x.CreatedOn) : o.OrderByDescending(x => x.CreatedOn),
            page : page,
            size : size);
    }

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
            include: q => q.Include(r => r.Creater).Include(r => r.Mentor)
            );
    }
}