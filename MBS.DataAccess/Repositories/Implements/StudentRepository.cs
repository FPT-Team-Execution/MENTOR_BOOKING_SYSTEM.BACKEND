using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class StudentRepository(IBaseDAO<Student> studentDao) : BaseRepository<Student>(studentDao), IStudentRepository
{
    public async Task<Pagination<Student>> GetPagingListAsync(int page, int size)
    {
        return await _dao.GetPagingListAsync(page: page, size: size);
    }

    public Task<Student> GetStudentByIdAsync(string id)
    {
        return _dao.SingleOrDefaultAsync(x => x.UserId == id, include: x => x.Include(x => x.User));
    }
}