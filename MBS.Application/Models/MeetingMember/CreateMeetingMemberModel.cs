namespace MBS.Application.Models.MeetingMember;

public class CreateMeetingMemberRequestModel
{
    public Guid MeetingId { get; set; }
    public string StudentId { get; set; }
    public DateTime JoinTime { get; set; }
}

public class CreateMeetingMemberResponseModel
{
	public Guid MeetingMemberId { get; set; }

}