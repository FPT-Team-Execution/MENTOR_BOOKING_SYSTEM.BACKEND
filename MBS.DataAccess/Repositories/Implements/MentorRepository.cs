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
    }
}