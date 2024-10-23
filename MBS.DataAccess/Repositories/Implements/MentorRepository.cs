using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.Repositories.Implements;

public class MentorRepository : BaseRepository<Mentor>, IMentorRepository
{
    public MentorRepository(IBaseDAO<Mentor> dao) : base(dao)
    {
    }

    public Task<Mentor?> GetByUserIdAsync(string userId, Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null)
    {
        return _dao.SingleOrDefaultAsync(
            predicate: x => x.UserId == userId,
            include: include
            );
    }

   

    public async Task<Mentor> GetMentorbyId(string id)
    {
        return await _dao.SingleOrDefaultAsync(e => e.UserId == id);
    }

    public async Task<IEnumerable<Mentor>> GetMentorsAsync()
    {
        return await _dao.GetListAsync();
    }
}