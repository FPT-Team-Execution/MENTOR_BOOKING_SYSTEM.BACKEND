using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Feedback;

// public class GetFeedbackRequestModel
// {
//     public IEnumerable<Core.Entities.Feedback> Feedbacks { get; set; } = new List<Core.Entities.Feedback>();
// }
public class GetMeetingFeedbacksByUserIdRequest
{
    [FromRoute]
    public Guid MeetingId { get; set; }
    [FromRoute]
    public required string UserId { get; set; }
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