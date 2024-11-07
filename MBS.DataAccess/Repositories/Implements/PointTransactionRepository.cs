using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class PointTransactionRepository : BaseRepository<PointTransaction>, IPointTransactionRepository
    {
        public PointTransactionRepository(IBaseDAO<PointTransaction> dao) : base(dao)
        {
        }

        public Task<Pagination<PointTransaction>> GetAllPageListAsync(int page, int size)
        {
            return _dao.GetPagingListAsync(
               page: page,
               size: size
        );
        }

        Task<Pagination<PointTransaction>> IPointTransactionRepository.GetTransactionByStudentIdPageList(string studentId, int page, int size)
        {
            return _dao.GetPagingListAsync(
                predicate: x => x.UserId == studentId,
                include: f => f.Include(f => f.User),
                page: page,
                size: size
         );
        }
    }
}
