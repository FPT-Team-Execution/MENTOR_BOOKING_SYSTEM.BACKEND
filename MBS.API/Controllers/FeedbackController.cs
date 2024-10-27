using MBS.Application.Models.Feedback;
using MBS.Application.Models.Meeting;
using MBS.Application.ValidationAttributes;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/feedbacks")]
[Authorize]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }
    
    // [HttpGet]
    // [ProducesResponseType(typeof(BaseModel<Pagination<Feedback>>),StatusCodes.Status200OK)]
    // [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetFeedbacks(int page, int size, DateTime? fromDate = null, DateTime? toDate = null)
    // {
    //     var result = await _feedbackService.GetFeedbacks(page, size, fromDate, toDate);
    //     return StatusCode(result.StatusCode, result);
    //     
    // }
    
    [HttpGet("meeting/{meetingId}/user/{userId}")]
    [CustomAuthorize(UserRoleEnum.Admin)]
    [ProducesResponseType(typeof(BaseModel<Pagination<FeedbackResponseDto>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbacksByUserId(GetMeetingFeedbacksByUserIdRequest request)
    {
        var result = await _feedbackService.GetMeetingFeedbacksByUserId(request);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("meeting/{meetingId}")]
    [CustomAuthorize(UserRoleEnum.Admin)]
    [ProducesResponseType(typeof(BaseModel<Pagination<FeedbackResponseDto>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbacksByMeetingId(GetFeedbacksByMeetingIdRequest request)
    {
        var result = await _feedbackService.GetFeedbacksByMeetingId(request);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{feedbackId}")]
    [ProducesResponseType(typeof(BaseModel<FeedbackResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbackById([FromRoute] Guid feedbackId)
    {
        var result = await _feedbackService.GetFeedbackById(feedbackId);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPost("")]
    [ProducesResponseType(typeof(BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateFeedback(CreateFeedbackRequestModel feedbackId)
    {
        var result = await _feedbackService.CreateFeedback(feedbackId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPut("{feedbackId}")]
    [ProducesResponseType(typeof(BaseModel<FeedbackResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateFeedback(Guid feedbackId, string message)
    {
        var result = await _feedbackService.UpdateFeedback(feedbackId, message);
        return StatusCode(result.StatusCode, result);
        
    }
}