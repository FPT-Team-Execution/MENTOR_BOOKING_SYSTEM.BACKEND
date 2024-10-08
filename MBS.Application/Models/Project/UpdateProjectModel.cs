using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Project;

public class UpdateProjectRequestModel
{
    [MaxLength(100), Required]
    public string  Title { get; set; }
    [MaxLength(200), Required]
    public string  Description { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    [MaxLength(50), Required]
    public string Semester { get; set; }
}
