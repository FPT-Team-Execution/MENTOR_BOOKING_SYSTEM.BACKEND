using MBS.Application.Models.Feedback;
using MBS.Application.Models.Progress;
using MBS.Application.ValidationAttributes;
using MBS.Core.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[Route("/api/progresses")]
[ApiController]
public class ProgressController : ControllerBase
{
    private readonly IProgressService _progressService;
    public ProgressController(IProgressService progressService)
    {
        _progressService = progressService;
    }
    [HttpGet("project/{projectId}")]
    [CustomAuthorize(UserRoleEnum.Admin)]
    [ProducesResponseType(typeof(BaseModel<GetProgressByProjectIddRequest>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProgressesByProjectId(GetProgressByProjectIddRequest request)
    {
        var result = await _progressService.GetProgressesByProjectId(request);
        return StatusCode(result.StatusCode, result);
        
    }
}