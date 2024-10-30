using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Application.ValidationAttributes;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("/api/calendar-events")]
[Authorize]
public class CalendarEventController : ControllerBase
{
    private readonly ICalendarEventService _calendarEventService;
    public CalendarEventController(ICalendarEventService calendarEventService)
    {
        _calendarEventService = calendarEventService;   
    }
   
    [HttpGet("mentor/{mentorId}")]
    [ProducesResponseType(typeof(BaseModel<Pagination<CalendarEvent>>),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventsByMentorIdPagination([FromRoute] string mentorId, [FromQuery]  string googleAccessToken, [FromQuery] CalendarEventPaginationQueryParameters parameters)
    {
        var result = await _calendarEventService.GetCalendarEventsByMentorId(mentorId, googleAccessToken, parameters);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("{calendarEventId}")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<CalendarEventResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCalendarById([FromRoute] string calendarEventId)
    {
        var result = await _calendarEventService.GetCalendarEventId(calendarEventId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("busy-event")]
    //[CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<GetBusyEventResponse, GetBusyEventRequest>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBusyEvents([FromQuery] GetBusyEventRequest request)
    {
        var result = await _calendarEventService.GetBusyEvent(request);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEvent([FromBody]CreateCalendarRequestModel requestModel)
    {
        var result = await _calendarEventService.CreateCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    
    [HttpPut("{calendarEventId}")]
    [CustomAuthorize(UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<UpdateCalendarEventResponseModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEvent([FromRoute] string calendarEventId, string accessToken, UpdateCalendarEventRequestModel requestModel)
    {
        var result = await _calendarEventService.UpdateCalendarEvent(calendarEventId,accessToken ,requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpDelete("{calendarEventId}")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<DeleteCalendarEventResponseModel>),StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEvent([FromRoute] string calendarEventId)
    {
        var result = await _calendarEventService.DeleteCalendarEvent(calendarEventId);
        return StatusCode(result.StatusCode, result);
        
    }
}