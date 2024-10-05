using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories;

public interface IUnitOfWork 
{
    MBSContext Context { get; }
    IBaseRepository<T> GetRepository<T>() where T : class;
    int Commit();
    Task<int> CommitAsync();
}