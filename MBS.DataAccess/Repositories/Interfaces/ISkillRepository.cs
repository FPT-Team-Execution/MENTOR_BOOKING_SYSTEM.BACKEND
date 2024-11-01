using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface ISkillRepository : IBaseRepository<Skill>
{
    Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId);

    Task<Skill> GetSkillByIdAsync(Guid id);

}