using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class RequestRepository(IBaseDAO<Request> dao) : BaseRepository<Request>(dao), IRequestRepository
{
    
}