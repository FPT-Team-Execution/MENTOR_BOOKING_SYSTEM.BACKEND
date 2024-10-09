using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.DAO;

public interface IUnitOfWork 
{
    MBSContext Context { get; }
    IBaseRepository<T> GetRepository<T>() where T : class;
    IBaseDAO<T> GetDAO<T>() where T : class;

    int Commit();
    Task<int> CommitAsync();
}