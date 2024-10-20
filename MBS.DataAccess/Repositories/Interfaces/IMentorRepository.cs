using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IMentorRepository : IBaseRepository<Mentor>
{
    Task<Mentor> GetMentorbyId(string id);
}