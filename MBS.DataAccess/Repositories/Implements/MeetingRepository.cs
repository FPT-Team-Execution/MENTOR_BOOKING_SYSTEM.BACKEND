using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class MeetingRepository(IBaseDAO<Meeting> dao) : BaseRepository<Meeting>(dao), IMeetingRepository
{
}