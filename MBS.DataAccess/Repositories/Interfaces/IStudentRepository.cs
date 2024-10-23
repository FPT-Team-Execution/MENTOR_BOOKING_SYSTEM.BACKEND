using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{

        Task<Pagination<Student>> GetStudentsAsync(int page, int size);
        Task<Student?> GetByUserIdAsync(string userId, Func<IQueryable<Student>, IIncludableQueryable<Student, object>> include = null);

        Task<IEnumerable<Student>> GetStudents();

}

