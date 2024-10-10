using MBS.DataAccess.DAO.Implements;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.DAO;

public class UnitOfWork : IUnitOfWork
{
    private Dictionary<Type, object> _repositories = new();

    public MBSContext Context { get; }
    public UnitOfWork(MBSContext context)
    {
        Context = context;
    }
    public DAO.Interfaces.IBaseRepository<T> GetRepository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out object repository))
        {
            return (DAO.Interfaces.IBaseRepository<T>)repository;
        }

        repository = new DAO.Implements.BaseRepository<T>(Context);
        _repositories.Add(typeof(T), repository);
        return (DAO.Interfaces.IBaseRepository<T>)repository;
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