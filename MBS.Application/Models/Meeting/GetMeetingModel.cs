namespace MBS.Application.Models.Meeting;

public class GetMeetingResponseModel
{
    public IEnumerable<Core.Entities.Meeting> Meetings { get; set; }
}