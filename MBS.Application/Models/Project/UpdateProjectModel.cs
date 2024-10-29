using System.ComponentModel.DataAnnotations;
using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;

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
    [EnumValidation(typeof(RequestStatusEnum))]
    public string Status { get; set; }
}
