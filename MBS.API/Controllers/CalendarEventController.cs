using MBS.Application.Helpers;
using MBS.Application.Models.CalendarEvent;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[ApiController]
[Route("api/calendar-event")]
public class CalendarEventController : ControllerBase
{
    private readonly ICalendarEventService _calendarEventService;
    public CalendarEventController(ICalendarEventService calendarEventService)
    {
        _calendarEventService = calendarEventService;   
    }
    [HttpPost("events")]
    public async Task<ActionResult<Task<BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>>>> CreateEvent(CreateCalendarRequestModel requestModel)
    {
        var result = await _calendarEventService.CreateCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }
    [HttpGet("events/mentors/{mentorId}")]
    public async Task<ActionResult<BaseModel<GetCalendarEventsResponseModel, GetCalendarEventsRequestModel>>> GetEvents(GetCalendarEventsRequestModel requestModel)
    {
        var result = await _calendarEventService.GetCalendarEventsByMentorId(requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpPut("events/{calendarEventId}")]
    public async Task<ActionResult<BaseModel<UpdateCalendarEventResponseModel, UpdateCalendarEventRequestModel>>> UpdateEvent(UpdateCalendarEventRequestModel requestModel)
    {
        var result = await _calendarEventService.UpdateCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpDelete("events/{calendarEventId}")]
    public async Task<ActionResult<BaseModel<DeleteCalendarEventResponseModel, DeleteCalendarEventRequestModel>>> DeleteEvent(DeleteCalendarEventRequestModel requestModel)
    {
        var result = await _calendarEventService.DeleteCalendarEvent(requestModel);
        return StatusCode(result.StatusCode, result);
        
    }
}