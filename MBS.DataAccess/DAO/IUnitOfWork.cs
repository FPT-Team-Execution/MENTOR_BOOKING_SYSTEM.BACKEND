using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.DAO;

public interface IUnitOfWork 
{
    //TODO: remove in future
    MBSContext Context { get; }
    DAO.Interfaces.IBaseRepository<T> GetRepository<T>() where T : class;
    //TODO: remove in future
    int Commit();
    //TODO: remove in furture
    Task<int> CommitAsync();
}