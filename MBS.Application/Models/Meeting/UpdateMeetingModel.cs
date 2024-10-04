using MBS.Core.Enums;

namespace MBS.Application.Models.Meeting;

public class UpdateMeetingRequestModel
{
    public string Description { get; set; }
    public string Location  { get; set; }
    public string MeetUp { get; set; }
    public MeetingStatusEnum  Status { get; set; }
}