using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class FeedBackRepository(IBaseDAO<Feedback> dao) : BaseRepository<Feedback>(dao), IFeedbackRepository
{
    
}