using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
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
    [HttpPost("")]
    public async Task<ActionResult<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>>> CreateEvent([FromBody]CreateCalendarRequestModel requestModel)
    {
        var result = await _calendarEventService.CreateCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    [HttpGet("{mentorId}")]
    public async Task<ActionResult<BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>>> GetEventByMentorId([FromRoute] string mentorId)
    {
        var result = await _calendarEventService.GetCalendarEventsByMentorId(mentorId);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPut("{calendarEventId}")]
    public async Task<ActionResult<BaseModel<UpdateCalendarEventResponseModel>>> UpdateEvent([FromRoute] string calendarEventId, UpdateCalendarEventRequestModel requestModel)
    {
        var result = await _calendarEventService.UpdateCalendarEvent(calendarEventId, requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpDelete("{calendarEventId}")]
    public async Task<ActionResult<BaseModel<DeleteCalendarEventResponseModel>>> DeleteEvent([FromRoute] string calendarEventId)
    {
        var result = await _calendarEventService.DeleteCalendarEvent(calendarEventId);
        return StatusCode(result.StatusCode, result);
        
    }
}