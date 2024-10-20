using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IFeedbackRepository : IBaseRepository<Feedback>
    {
        Task<Pagination<Feedback>> GetFeedbackPagingAsync(int page, int size, Expression<Func<Feedback, bool>> predicate = null, Func<IQueryable<Feedback>, IIncludableQueryable<Feedback, object>> include = null);


        Task<Feedback> GetFeedbackByIdAsync(Guid id);
    }
}
