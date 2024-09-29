using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class MeetingRepository : BaseRepository<Meeting>, IMeetingRepository
{
    public MeetingRepository(MBSContext context) : base(context)
    {
    }
}