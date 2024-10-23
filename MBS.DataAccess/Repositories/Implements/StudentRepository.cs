using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.Repositories.Implements;

public class StudentRepository2(IBaseDAO<Student> dao) : BaseRepository<Student>(dao), IStudentRepository
{
    public Task<Pagination<Student>> GetStudentsAsync(int page, int size)
    {
        throw new NotImplementedException();
    }

    public Task<Student?> GetByUserIdAsync(string userId, Func<IQueryable<Student>, IIncludableQueryable<Student, object>> include = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Student>> GetStudents()
    {
        throw new NotImplementedException();
    }
}