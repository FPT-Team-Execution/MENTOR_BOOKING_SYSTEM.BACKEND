using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class SkillRepository(IBaseDAO<Skill> dao) : BaseRepository<Skill>(dao), ISkillRepository
{
    public async Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId)
    {
        return await _dao.GetPagingListAsync(
            predicate: s => s.MentorId == mentorId,
            page:page,
            size:size
        );
    }
}