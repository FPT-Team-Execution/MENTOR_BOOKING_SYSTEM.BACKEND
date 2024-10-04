namespace MBS.Application.Models.Feedback;

public class CreateFeedbackRequestModel
{
    public Guid MeetingId { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
}

public class CreateFeedbackResponseModel
{
    public string FeedbackId { get; set; }

}