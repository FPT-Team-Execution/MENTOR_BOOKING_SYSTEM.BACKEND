using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private Dictionary<Type, object> _repositories = new();
    public MBSContext Context { get; }
    public IBaseRepository<T> GetRepository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out object repository))
        {
            return (IBaseRepository<T>)repository;
        }

        repository = new BaseRepository<T>(Context);
        _repositories.Add(typeof(T), repository);
        return (IBaseRepository<T>)repository;
    }

    public int Commit()
    {
        return Context.SaveChanges();
    }

    public Task<int> CommitAsync()
    {
        return Context.SaveChangesAsync();
    }
}