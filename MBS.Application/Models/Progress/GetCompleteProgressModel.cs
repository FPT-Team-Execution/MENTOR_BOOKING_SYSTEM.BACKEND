using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Progress;

public class GetCompleteProgressRequest
{
    [FromQuery]
    public required Guid ProjectId { get; set; }
}

public class GetCompleteProgressResponse
{
    public double Percent { get; set; }
    public IEnumerable<ProgressResponseDto> Complete { get; set; }
    public IEnumerable<ProgressResponseDto> NotComplete { get; set; }

}