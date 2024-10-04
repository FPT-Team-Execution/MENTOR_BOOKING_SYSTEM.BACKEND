using MBS.Application.Models.Meeting;
using MBS.Application.Models.MeetingMember;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    private readonly IMeetingMemberService _meetingMemberService;
    public MeetingController(IMeetingService meetingService, IMeetingMemberService meetingMemberService)
    {
        _meetingService = meetingService;
        _meetingMemberService = meetingMemberService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> GetMeetings()
    {
        var result = await _meetingService.GetMeetings();
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{meetingId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> GetMeetingById([FromRoute] Guid meetingId)
    {
        var result = await _meetingService.GetMeetingId(meetingId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel, CreateMeetingRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel, CreateMeetingRequestModel>>> CreateRequest([FromBody]CreateMeetingRequestModel requestModel)
    {
        var result = await _meetingService.CreateMeeting(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    
    [HttpPut("{meetingId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> UpdateEvent([FromRoute] Guid meetingId, UpdateMeetingRequestModel requestModel)
    {
        var result = await _meetingService.UpdateMeeting(meetingId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPost("members")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> AddMemberMeeting(CreateMeetingMemberRequestModel requestModel)
    {
        var result = await _meetingMemberService.AddMemberMeeting(requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("{meetingId}/members")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> GetMeetingMembersByMeetingId(Guid meetingId)
    {
        var result = await _meetingMemberService.GetMembersByMeetingId(meetingId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("{meetingId}/members")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> UpdateMeetingMember(Guid memberMeetingId, UpdateMeetingMemberRequestModel request)
    {
        var result = await _meetingMemberService.UpdateMeetingMember(memberMeetingId, request);
        return StatusCode(result.StatusCode, result);
        
    }
}