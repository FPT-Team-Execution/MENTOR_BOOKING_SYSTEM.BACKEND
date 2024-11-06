using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.Progress;

public class CreateProgressesRequest
{
    public required Guid ProjectId { get; set; }
    public IEnumerable<string> Names { get; set; }
}

public class CreateProgressesResponse
{
    public required Guid ProgressId { get; set; }
}