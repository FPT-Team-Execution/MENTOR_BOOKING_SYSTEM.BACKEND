using MBS.DataAccess.DAO.Implements;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.DAO;

public class UnitOfWork : IUnitOfWork
{
    private Dictionary<Type, object> _repositories = new();
    private Dictionary<Type, object> _daoInstances = new();

    public MBSContext Context { get; }
    public UnitOfWork(MBSContext context)
    {
        Context = context;
    }
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

    public IBaseDAO<T> GetDAO<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out object daoInstance))
        {
            return (IBaseDAO<T>)daoInstance;
        }

        daoInstance = new BaseDAO<T>(Context);
        _repositories.Add(typeof(T), daoInstance);
        return (IBaseDAO<T>)daoInstance;
    }

    public int Commit()
    {
        return Context.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await Context.SaveChangesAsync();
    }
}