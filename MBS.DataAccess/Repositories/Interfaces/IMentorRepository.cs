using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMentorRepository : IBaseRepository<Mentor>    
{
    Task<Mentor?> GetMentorByIdAsync(string mentorId);
    Task<Pagination<Mentor>> GetMentorsAsync(int page, int size);
}