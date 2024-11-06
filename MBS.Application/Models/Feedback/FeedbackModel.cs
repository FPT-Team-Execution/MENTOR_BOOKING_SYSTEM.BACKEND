using MBS.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Feedback;

public class FeedbackModel
{
    public FeedbackResponseDTO Feedback { get; set; }
}

public class FeedbackResponseDTO
{
    public Guid Id { get; set; }
	public string userId { get; set; }
	public string name { get; set; }
    public Guid MeetingId { get; set; }
	public string? Message { get; set; }
	public string? CreatedBy { get; set; }
	public DateTime? CreatedOn { get; set; }
	public string? UpdatedBy { get; set; }
	public DateTime? UpdatedOn { get; set; }
}