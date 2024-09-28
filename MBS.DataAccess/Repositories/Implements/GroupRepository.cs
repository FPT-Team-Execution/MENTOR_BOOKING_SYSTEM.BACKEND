using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class GroupRepository : BaseRepository<Group>, IGroupRepository  
{
    public GroupRepository(MBSContext context) : base(context)
    {
    }
}