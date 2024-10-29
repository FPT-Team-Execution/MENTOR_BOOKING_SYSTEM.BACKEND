using MBS.Application.Models.Meeting;
using MBS.Application.Models.MeetingMember;
using MBS.Application.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/meeting-members")]
[Authorize]
public class MeetingMemberController : ControllerBase
{
    private readonly IMeetingMemberService _meetingMemberService;

    public MeetingMemberController(IMeetingMemberService meetingMemberService)
    {
        _meetingMemberService = meetingMemberService;
    }
    
    [HttpGet("meeting/{meetingId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMeetingMembersByMeetingId(Guid meetingId)
    {
        var result = await _meetingMemberService.GetMembersByMeetingId(meetingId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddMemberMeeting(CreateMeetingMemberRequestModel requestModel)
    {
        var result = await _meetingMemberService.AddMemberMeeting(requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPut("{meetingMemberId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateMeetingMember(Guid meetingMemberId, UpdateMeetingMemberRequestModel request)
    {
        var result = await _meetingMemberService.UpdateMeetingMember(meetingMemberId, request);
        return StatusCode(result.StatusCode, result);
        
    }
}