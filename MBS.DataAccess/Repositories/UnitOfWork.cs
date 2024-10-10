
using MBS.DataAccess.DAO.Interfaces;
namespace MBS.DataAccess.Repositories;

public class UnitOfWork<T> : IUnitOfWork<T> where T : class
{

    private Dictionary<Type, object> _repositories = new();
    private readonly IBaseDAO<T> _dao;
    public UnitOfWork(IBaseDAO<T> dao)
    {
        _dao = dao;
    }
    
    public Interfaces.IBaseRepository<T> GetRepository() 
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new Implements.BaseRepository<T>(_dao);  
            _repositories[type] = repositoryInstance;
        }

        return (Interfaces.IBaseRepository<T>)_repositories[type];
    }
    
}