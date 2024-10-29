using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IDegreeRepository : IBaseRepository<Degree>
{
    Task<Pagination<Degree>> GetDegreesByMentorId(string mentorId, int page, int size);
}