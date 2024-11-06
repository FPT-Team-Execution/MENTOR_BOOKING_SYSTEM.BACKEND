using MBS.Core.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Progress;

public class GetProgressByProjectIddRequest
{
    [FromRoute(Name = "projectId")]
    public required Guid ProjectId { get; set; }
    [FromQuery] public required int Page { get; set; } = 1;
    [FromQuery] public required int Size { get; set; } = 10;

}

public class GetProgressByProjectIddResponse
{
    public Pagination<ProgressResponseDto> Progresses { get; set; }
}