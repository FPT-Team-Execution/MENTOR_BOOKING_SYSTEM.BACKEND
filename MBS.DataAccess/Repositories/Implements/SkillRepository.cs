using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class SkillRepository(IBaseDAO<Skill> skillDao) : BaseRepository<Skill>(skillDao), ISkillRepository
{

    public async Task<Skill?> GetByIdAsync(Guid id)
    {
        return await _dao.SingleOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Get page list async with predicate
    /// </summary>
    /// <param name="page">page index</param>
    /// <param name="size">size of item per page</param>
    /// <param name="skillId">skill Id to predicate</param>
    /// <returns>Pagination<Skill></returns>
    public async Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId)
    {
        return await _dao.GetPagingListAsync(
            predicate: s => s.MentorId == mentorId,
            page:page,
            size:size
        );
    }
}