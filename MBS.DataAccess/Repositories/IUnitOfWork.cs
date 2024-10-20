using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories;

public interface IUnitOfWork<T> where T : class
{
     ISkillRepository SkillRepository { get; set; }
}