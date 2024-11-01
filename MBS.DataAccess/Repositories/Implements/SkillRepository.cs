using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class SkillRepository(IBaseDAO<Skill> dao) : BaseRepository<Skill>(dao), ISkillRepository
{
    public Task<Skill?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId)
    {
        return await _dao.GetPagingListAsync(
            predicate: s => s.MentorId == mentorId,
            page:page,
            size:size
        );
    }

    public async Task<Skill> GetSkillByIdAsync(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(
            m => m.Id == id,
            include: x => x.Include(y => y.Mentor)

            );
    }
}