<<<<<<< HEAD
=======
using MBS.Core.Common.Pagination;
>>>>>>> develop
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

<<<<<<< HEAD
public interface IMentorRepository : IBaseRepository<Mentor>
{
    Task<Mentor> GetMentorbyId(string id);
=======
public interface IMentorRepository : IBaseRepository<Mentor>    
{
    Task<Mentor?> GetMentorByIdAsync(string mentorId);
>>>>>>> develop
}