using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class FeedbackRepository : BaseRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IBaseDAO<Feedback> dao) : base(dao)
        {
        }

        public async Task<Feedback> GetFeedbackByIdAsync(Guid id)
        {
            return await _dao.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Pagination<Feedback>> GetFeedbackPagingAsync(int page, int size, Expression<Func<Feedback, bool>> predicate = null, Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>> include = null)
        {
            return await _dao.GetPagingListAsync(
                predicate: predicate,
                page: page,
                size: size,
                include: include
                );
        }

        
    }
}
