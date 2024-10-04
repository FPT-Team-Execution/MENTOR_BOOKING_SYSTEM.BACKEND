using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class MeetingMemberRepository(MBSContext context)
    : BaseRepository<MeetingMember>(context), IMeetingMemberRepository;