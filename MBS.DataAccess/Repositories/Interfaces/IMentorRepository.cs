using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMentorRepository : IBaseRepository<Mentor>    
{
    Task<IEnumerable<Mentor>> GetMentorsAsync();
    Task<Mentor?> GetByUserIdAsync(string userId,
   Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null);
    Task<Mentor?> GetMentorByIdAsync(string mentorId);
    Task<Pagination<Mentor>> GetMentorsAsync(int page, int size);
}