using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Progress;

public class DeleteProgressRequest
{
    [FromRoute(Name = "progressId")]
    public Guid ProgressId { get; set; }
}