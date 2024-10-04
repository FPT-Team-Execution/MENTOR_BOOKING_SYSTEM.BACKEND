namespace MBS.Application.Models.MeetingMember;

public class GetMeetingMemberResponseModel
{
    public IEnumerable<Core.Entities.MeetingMember> MeetingMembers { get; set; } = new List<Core.Entities.MeetingMember>();
}