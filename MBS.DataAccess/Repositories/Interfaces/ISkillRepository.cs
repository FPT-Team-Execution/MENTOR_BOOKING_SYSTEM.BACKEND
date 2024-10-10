using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface ISkillRepository : IBaseRepository<Skill>
{
    Task<Skill?> GetByIdAsync(Guid id);
    Task<Pagination<Skill>> GetPagedListAsyncByMentorId(int page, int size, string mentorId);

}