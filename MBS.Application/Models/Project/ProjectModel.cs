using MBS.Core.Entities;
using MBS.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Project;

public class ProjectResponseModel
{
    public ProjectResponseDto Project { get; set; }
}
 public class ProjectResponseDto
{
	public string Title { get; set; }
	public string Description { get; set; }
	public DateTime DueDate { get; set; }
	public string Semester { get; set; }
	public string? CreatedBy { get; set; }
	public string MentorId { get; set; }
	public ProjectStatusEnum Status { get; set; }
}