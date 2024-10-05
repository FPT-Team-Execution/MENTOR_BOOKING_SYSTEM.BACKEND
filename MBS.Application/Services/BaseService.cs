using MBS.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services;

public class BaseService <T> where T : class
{

    protected IUnitOfWork _unitOfWork;
    protected ILogger<T> _logger;

    public BaseService(IUnitOfWork unitOfWork, ILogger<T> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;

    }

}