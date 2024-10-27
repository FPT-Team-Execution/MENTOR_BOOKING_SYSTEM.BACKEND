using MBS.Application.Models.Feedback;
using MBS.Application.Models.Meeting;
using MBS.Application.Models.MeetingMember;
using MBS.Application.ValidationAttributes;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/meetings")]
[Authorize]
public class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    private readonly IMeetingMemberService _meetingMemberService;
    private readonly IFeedbackService _feedbackService;
    public MeetingController(IMeetingService meetingService, IMeetingMemberService meetingMemberService, IFeedbackService feedbackService)
    {
        _meetingService = meetingService;
        _meetingMemberService = meetingMemberService;
        _feedbackService = feedbackService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMeetings(int page, int size)
    {
        var result = await _meetingService.GetMeetings(page, size);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{meetingId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMeetingById([FromRoute] Guid meetingId)
    {
        var result = await _meetingService.GetMeetingId(meetingId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel, CreateMeetingRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateMeeting([FromBody]CreateMeetingRequestModel requestModel)
    {
        var result = await _meetingService.CreateMeeting(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    
    [HttpPut("{meetingId}")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMeeting([FromRoute] Guid meetingId, UpdateMeetingRequestModel requestModel)
    {
        var result = await _meetingService.UpdateMeeting(meetingId, requestModel);
        return StatusCode(result.StatusCode, result);
    }
}