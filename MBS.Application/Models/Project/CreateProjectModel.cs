using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Project;

public class CreateProjectRequestModel
{
    [Required, MaxLength(100)]
    public string Title { get; set; }
    [Required, MaxLength(200)]
    public string Description { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [Required, MaxLength(50)]
    public string Semester { get; set; }
    [Required, MaxLength(450)]
    public string MentorId { get; set; }

}

public class CreateProjectResponseModel
{
    public Guid ProjectId { get; set; }
}