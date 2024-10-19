<<<<<<< HEAD
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class MentorRepository : BaseRepository<Mentor>, IMentorRepository
{
    public MentorRepository(IBaseDAO<Mentor> dao) : base(dao)
    {
    }
    
    public async Task<Mentor> GetMentorbyId(string id)
    {
        return await _dao.SingleOrDefaultAsync(e => e.UserId == id);
=======
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
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
>>>>>>> develop
    }
}