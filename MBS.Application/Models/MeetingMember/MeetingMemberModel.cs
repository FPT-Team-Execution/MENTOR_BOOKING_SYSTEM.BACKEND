using MBS.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.MeetingMember;

public class MeetingMemberResponseModel
{
    public MeetingMemberResponseDto MeetingMember { get; set; }
}

public class MeetingMemberResponseDto
{
    public Guid Id { get; set; }
    public Guid MeetingId { get; set; }
	public string StudentId { get; set; }
	public DateTime JoinTime { get; set; }
	public DateTime? LeaveTime { get; set; }
}