using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Progress;

public class CreateProgressRequest
{
    public required Guid ProjectId { get; set; }
    [MaxLength(100)]
    public string Name  { get; set; }
}

public class CreateProgressResponse
{
    public required Guid ProgressId { get; set; }
}