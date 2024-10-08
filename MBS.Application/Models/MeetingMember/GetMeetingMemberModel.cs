namespace MBS.Application.Models.MeetingMember;

public class GetMeetingMemberResponseModel
{
    public IEnumerable<MeetingMemberResponseDto> MeetingMembers { get; set; } = new List<MeetingMemberResponseDto>();
}

