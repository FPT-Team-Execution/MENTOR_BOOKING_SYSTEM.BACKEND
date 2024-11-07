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
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor, UserRoleEnum.Student)]
    [ProducesResponseType(typeof(BaseModel<GetProgressByProjectIddRequest>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProgressesByProjectId(GetProgressByProjectIddRequest request)
    {
        var result = await _progressService.GetProgressesByProjectId(request);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("get-complete")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor, UserRoleEnum.Student)]
    [ProducesResponseType(typeof(BaseModel<GetProgressByProjectIddRequest>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCompleteProgressPercent(GetCompleteProgressRequest request)
    {
        var result = await _progressService.GetCompleteProgressPercent(request);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPost("")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Student)]
    [ProducesResponseType(typeof(BaseModel<CreateProgressResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProgress(CreateProgressRequest request)
    {
        var result = await _progressService.CreateProgress(request);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPut("")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Student)]
    [ProducesResponseType(typeof(BaseModel<UpdateProgressResponse>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProgress([FromBody]UpdateProgressRequest request)
    {
        var result = await _progressService.UpdateProgress(request);
        return StatusCode(result.StatusCode, result);
    }
    [HttpDelete("{progressId}")]
    [CustomAuthorize(UserRoleEnum.Admin)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProgress(DeleteProgressRequest request)
    {
        var result = await _progressService.DeleteProgress(request);
        return StatusCode(result.StatusCode, result);
        
    }
}