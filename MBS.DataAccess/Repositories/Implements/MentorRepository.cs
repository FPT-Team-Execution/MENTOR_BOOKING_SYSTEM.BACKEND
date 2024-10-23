using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class MentorRepository(IBaseDAO<Mentor> dao) : BaseRepository<Mentor>(dao), IMentorRepository
{
    public async Task<Mentor?> GetMentorByIdAsync(string mentorId)
    {
        return await _dao.SingleOrDefaultAsync(
            predicate: m => m.UserId == mentorId,
            include: source => source.Include(m => m.User)
        );
    }

    public Task<Mentor?> GetByUserIdAsync(string userId, Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null)
    {
        return _dao.SingleOrDefaultAsync(
            predicate: x => x.UserId == userId,
            include: include
            );
    }

    public async Task<Pagination<Mentor>> GetMentorsAsync(int page, int size)
    {
        return await _dao.GetPagingListAsync(
            include: source => source.Include(m => m.User)
        );
    }
    
    public async Task<IEnumerable<Mentor>> GetMentorsAsync()
    {
        return await _dao.GetListAsync();
    }
}