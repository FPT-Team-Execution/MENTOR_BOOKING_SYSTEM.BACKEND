using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Progress;

public class UpdateProgressRequest
{
    public required Guid ProgressId { get; set; }
    [MaxLength(100)]
    public required  string Name  { get; set; }
    public required bool IsComplete { get; set; }
}
public class UpdateProgressResponse
{
    public ProgressResponseDto Progress {
        get;
        set;
    }
}