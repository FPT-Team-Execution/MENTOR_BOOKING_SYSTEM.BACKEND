namespace MBS.Application.Models.Meeting;

public class CreateMeetingRequestModel
{
    public Guid RequestId { get; set; }
    public string Description { get; set; }
    public string Location  { get; set; }
    public string MeetUp { get; set; }
}

public class CreateMeetingResponseModel
{
	public Guid RequestId { get; set; }
}