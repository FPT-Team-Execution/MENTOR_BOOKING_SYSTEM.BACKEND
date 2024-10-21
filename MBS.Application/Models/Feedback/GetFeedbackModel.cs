namespace MBS.Application.Models.Feedback;

public class GetFeedbackRequestModel
{
    public IEnumerable<Core.Entities.Feedback> Feedbacks { get; set; } = new List<Core.Entities.Feedback>();
}