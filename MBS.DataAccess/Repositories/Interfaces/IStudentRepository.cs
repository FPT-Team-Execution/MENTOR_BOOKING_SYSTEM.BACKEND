using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{
    Task<Pagination<Student>> GetPagingListAsync(int page, int size);
    Task<Student> GetStudentByIdAsync(string id);
}