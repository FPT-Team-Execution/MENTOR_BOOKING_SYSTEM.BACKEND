
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories;

public class UnitOfWork<T> : IUnitOfWork<T> where T : class
{
    public ISkillRepository SkillRepository { get; set; }
    public UnitOfWork(IBaseDAO<T> dao)
    {
        SkillRepository = new SkillRepository((IBaseDAO<Skill>)dao);

    }


}