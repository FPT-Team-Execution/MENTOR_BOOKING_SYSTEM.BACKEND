using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Feedback;

public class CreateFeedbackRequestModel
{
    [Required]
    public Guid MeetingId { get; set; }
    public required string UserId { get; set; }
    public required string Message { get; set; }
}

public class CreateFeedbackResponseModel
{
    public string FeedbackId { get; set; }

}