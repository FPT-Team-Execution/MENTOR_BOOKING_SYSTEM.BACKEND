using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMentorRepository : IBaseRepository<Mentor>
{
    Task<Mentor> GetMentorbyId(string id);
    Task<IEnumerable<Mentor>> GetMentorsAsync();

    Task<Mentor?> GetByUserIdAsync(string userId,
   Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>> include = null);
}