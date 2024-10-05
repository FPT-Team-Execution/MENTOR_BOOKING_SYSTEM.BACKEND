using MBS.Application.Models.Feedback;
using MBS.Application.Models.Meeting;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/feedbacks")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }
    [HttpGet("meeting/{meetingId}/user/{userId}")]
    [ProducesResponseType(typeof(BaseModel<Pagination<Feedback>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbackByUserId([FromRoute] Guid meetingId, [FromRoute] string userId, int page, int size)
    {
        var result = await _feedbackService.GetFeedbacksByUserId(meetingId, userId, page, size);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("meeting/{meetingId}")]
    [ProducesResponseType(typeof(BaseModel<Pagination<Feedback>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeedbacksInMeeting([FromRoute] Guid meetingId, int page, int size)
    {
        var result = await _feedbackService.GetFeedbacksByMeetingId(meetingId, page, size);
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
    public async Task<IActionResult> CreateFeedback( CreateFeedbackRequestModel feedbackId)
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