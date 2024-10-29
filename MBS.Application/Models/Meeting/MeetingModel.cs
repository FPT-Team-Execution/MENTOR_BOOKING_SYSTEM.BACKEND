using MBS.Core.Enums;

namespace MBS.Application.Models.Meeting;

public class MeetingResponseModel
{
    public MeetingResponseDto Meeting { get; set; }
}
public class MeetingResponseDto
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
	public string Description { get; set; }
	public string Location { get; set; }
	public string MeetUp { get; set; }
	public MeetingStatusEnum Status { get; set; }
}