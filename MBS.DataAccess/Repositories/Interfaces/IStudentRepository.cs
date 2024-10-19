using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
<<<<<<< HEAD
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<Pagination<Student>> GetStudentsAsync(int page, int size);
        Task<Student?> GetByUserIdAsync(string userId);


        
    }
}
=======

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IStudentRepository : IBaseRepository<Student>
{
}
>>>>>>> develop
