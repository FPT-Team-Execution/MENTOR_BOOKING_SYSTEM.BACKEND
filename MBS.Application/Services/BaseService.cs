using AutoMapper;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services;

public class BaseService <T> where T : class
{

    protected IUnitOfWork _unitOfWork;
    protected ILogger<T> _logger;
    protected IMapper _mapper;
    public BaseService(IUnitOfWork unitOfWork, ILogger<T> logger, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_mapper = mapper;
	}

}

public class BaseService2<T, TService> where T : class where TService : class 
{

	protected IUnitOfWork<T> _unitOfWork;
	protected ILogger<TService> _logger;
	protected IMapper _mapper;
	public BaseService2(IUnitOfWork<T> unitOfWork, ILogger<TService> logger, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_mapper = mapper;
	}

}