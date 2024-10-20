using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IBaseDAO<Student> studentDao) : base(studentDao)
        {
        }

        public async Task<Pagination<Student>> GetStudentsAsync(int page, int size)
        {
            return await _dao.GetPagingListAsync(
                include: s => s.Include(x => x.User),
                page: page,
                size: size
            );
        }

        public async Task<Student?> GetByUserIdAsync(string userId)
        {
            return await _dao.SingleOrDefaultAsync(x => x.UserId == userId);
        }

        
    }
}
