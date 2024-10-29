using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class FeedBackRepository(IBaseDAO<Feedback> dao) : BaseRepository<Feedback>(dao), IFeedbackRepository
{
    public Task<Pagination<Feedback>> GetMeetingFeedBacksByUserId(Guid meetingId, string userId, int page, int size, string sortBy)
    {
        return _dao.GetPagingListAsync(
            predicate: f => f.MeetingId == meetingId && f.UserId == userId,
            include: q => q.Include(f => f.User),
            orderBy: q => (sortBy.ToLower() == "asc" ? q.OrderBy(f => f.CreatedOn) : q.OrderByDescending(f => f.CreatedOn)),
            page: page,
            size: size
            );
    }
    public Task<Pagination<Feedback>> GetFeedBacksByMeetingId(Guid meetingId,int page, int size, string sortBy)
    {
        return _dao.GetPagingListAsync(
            predicate: f => f.MeetingId == meetingId,
            include: q => q.Include(f => f.User),
            orderBy: q => (sortBy.ToLower() == "asc" ? q.OrderBy(f => f.CreatedOn) : q.OrderByDescending(f => f.CreatedOn)),
            page: page,
            size: size
        );
    }
}