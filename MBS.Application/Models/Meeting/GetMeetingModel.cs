using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Meeting;

public class GetMeetingFeedbacksByUserIdRequest
{
    [FromRoute]
    public Guid MeetingId { get; set; }
    [FromRoute]
    public string UserId { get; set; }
    [FromQuery]
    public int Page { get; set; } = 1;
    [FromQuery]
    public int Size { get; set; } = 10;
    [FromQuery]
    public string SortOrder { get; set; } = "asc";
}

public class GetFeedbacksByMeetingIdRequest
{
    [FromRoute]
    public Guid MeetingId { get; set; }
    [FromQuery]
    public int Page { get; set; } = 1;
    [FromQuery]
    public int Size { get; set; } = 10;
    [FromQuery]
    public string SortOrder { get; set; } = "asc";
}
