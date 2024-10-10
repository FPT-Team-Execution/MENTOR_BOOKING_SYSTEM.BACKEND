using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories;

public interface IUnitOfWork<T> where T : class
{
    IBaseRepository<T> GetRepository();
}