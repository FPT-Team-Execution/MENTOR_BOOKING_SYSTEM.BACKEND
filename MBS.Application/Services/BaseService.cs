using AutoMapper;
using MBS.DataAccess.DAO;
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