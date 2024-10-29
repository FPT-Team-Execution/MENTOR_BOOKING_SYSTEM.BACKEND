using MBS.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Feedback;

public class FeedbackResponseModel
{
    public FeedbackResponseDto Feedback { get; set; }
}

public class FeedbackResponseDto
{
    public Guid Id { get; set; }
    public Guid MeetingId { get; set; }
	public string? Message { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? CreatedOn { get; set; }
	public string? UpdatedBy { get; set; }
	public DateTime? UpdatedOn { get; set; }
}