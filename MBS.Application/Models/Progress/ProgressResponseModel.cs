using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Progress;

public class ProgressResponseDto
{
    public Guid Id{ get; set; }
    public required string Name { get; set; }
    public bool IsComplete { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}