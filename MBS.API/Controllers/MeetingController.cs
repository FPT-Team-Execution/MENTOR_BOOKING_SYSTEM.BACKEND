using MBS.Application.Models.Meeting;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;
    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> GetMeetings()
    {
        var result = await _meetingService.GetMeetings();
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{requestId}")]
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
    
    [HttpPut("{requestId}")]
    [ProducesResponseType(typeof(BaseModel<MeetingResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<MeetingResponseModel>>> UpdateEvent([FromRoute] Guid requestId, UpdateMeetingRequestModel requestModel)
    {
        var result = await _meetingService.UpdateMeeting(requestId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
}