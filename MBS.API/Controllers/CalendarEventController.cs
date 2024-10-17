using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("/api/calendar-event")]
public class CalendarEventController : ControllerBase
{
    private readonly ICalendarEventService _calendarEventService;
    public CalendarEventController(ICalendarEventService calendarEventService)
    {
        _calendarEventService = calendarEventService;   
    }
   
    // [HttpGet("mentor/{mentorId}")]
    // [ProducesResponseType(typeof(BaseModel<Pagination<CalendarEvent>>),StatusCodes.Status204NoContent)]
    // [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    // [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    // [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    // public async Task<ActionResult<BaseModel<DeleteCalendarEventResponseModel>>> GetEventsByMentorIdPagination([FromRoute] string mentorId, int page, int size)
    // {
    //     var result = await _calendarEventService.GetCalendarEventsByMentorIdPagination(mentorId, page, size);
    //     return StatusCode(result.StatusCode, result);
    //     
    // }
    [HttpGet("{calendarEventId}")]
    [ProducesResponseType(typeof(BaseModel<CalendarEventResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<CalendarEventResponseModel>>> GetRequestById([FromRoute] string calendarEventId)
    {
        var result = await _calendarEventService.GetCalendarEventId(calendarEventId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [ProducesResponseType(typeof(BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>>> CreateEvent([FromBody]CreateCalendarRequestModel requestModel)
    {
        var result = await _calendarEventService.CreateCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    
    [HttpPut("{calendarEventId}")]
    [ProducesResponseType(typeof(BaseModel<UpdateCalendarEventResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<UpdateCalendarEventResponseModel>>> UpdateEvent([FromRoute] string calendarEventId, UpdateCalendarEventRequestModel requestModel)
    {
        var result = await _calendarEventService.UpdateCalendarEvent(calendarEventId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpDelete("{calendarEventId}")]
    [ProducesResponseType(typeof(BaseModel<DeleteCalendarEventResponseModel>),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<BaseModel<DeleteCalendarEventResponseModel>>> DeleteEvent([FromRoute] string calendarEventId)
    {
        var result = await _calendarEventService.DeleteCalendarEvent(calendarEventId);
        return StatusCode(result.StatusCode, result);
        
    }
}