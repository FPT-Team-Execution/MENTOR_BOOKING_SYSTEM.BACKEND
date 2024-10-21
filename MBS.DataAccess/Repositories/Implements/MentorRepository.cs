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
    }
}