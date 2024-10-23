using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore.Query;
using System.Threading.Tasks;
=======
>>>>>>> parent of 4cb5763 (merge query to test api with data)

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{
<<<<<<< HEAD
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Pagination<Student>> GetStudentsAsync(int page, int size);
        Task<Student?> GetByUserIdAsync(string userId,
    Func<IQueryable<Student>, IIncludableQueryable<Student, object>> include = null);

        Task<IEnumerable<Student>> GetStudents();


    }
}
=======
}
>>>>>>> parent of 4cb5763 (merge query to test api with data)
