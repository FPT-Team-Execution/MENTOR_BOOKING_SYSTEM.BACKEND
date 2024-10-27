using MBS.Application.Helpers;
using MBS.Shared.Models.Google.GoogleCalendar.Request;
using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Services.Interfaces;

namespace MBS.API.Controllers;

[Route("api/calendars")]
[ApiController]
//TODO: not show google calendar api
[ApiExplorerSettings(IgnoreApi = true)]
public class GoogleCalendarController : ControllerBase
{
    private readonly IGoogleService _googleService;

    public GoogleCalendarController(IGoogleService googleService)
    {
        _googleService = googleService;
    }

    [HttpGet("{calendarId}/events")]
    public async Task<IActionResult> ListEvents([FromRoute] string calendarId, [FromQuery] string accessToken, DateTime timeMax, DateTime timeMin)
    {
        if (string.IsNullOrWhiteSpace(calendarId) || string.IsNullOrWhiteSpace(accessToken) || timeMin >= timeMax)
        {
            return BadRequest(MessageResponseHelper.InvalidInputParameter());
        }

        var request = new GetGoogleCalendarEventsRequest
        {
            Email = calendarId,
            AccessToken = accessToken,
            TimeMax = timeMax,
            TimeMin = timeMin
        };

        var result = await _googleService.ListEvents(request);
        if(result.IsSuccess){
            return StatusCode(StatusCodes.Status200OK, result);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
    }
    [HttpPost("freebusy")]
    public async Task<IActionResult> GetFreeBusy(FreeBusyParamters request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.AccessToken) )
        {
            return BadRequest(MessageResponseHelper.InvalidInputParameter());
        }

        var result = await _googleService.GetFreeBusyPeriod(request);
        if(result.IsSuccess){
            return StatusCode(StatusCodes.Status200OK, result);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
    }
    [HttpPost("{calendarId}/events")]
    public async Task<IActionResult> InsertEvent([FromRoute]string calendarId, [FromQuery] string accessToken, [FromBody]CreateGoogleCalendarEventRequest requestBody)
    {
        if (string.IsNullOrWhiteSpace(calendarId) || string.IsNullOrWhiteSpace(accessToken) || requestBody.End < requestBody.Start)
        {
            return BadRequest(MessageResponseHelper.InvalidInputParameter());
        }
    
        var result = await _googleService.InsertEvent(calendarId, accessToken, requestBody);
        if(result.IsSuccess){
            return StatusCode(StatusCodes.Status200OK, result);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
        
    }
    
    [HttpPut("{calendarId}/event/{eventId}")]
    public async Task<IActionResult> UpdateEvent([FromRoute]string calendarId, [FromRoute] string eventId, [FromQuery] string accessToken, [FromBody]UpdateGoogleCalendarEventRequest requestBody)
    {
        if (string.IsNullOrWhiteSpace(calendarId) || string.IsNullOrWhiteSpace(accessToken) || requestBody.End < requestBody.Start)
        {
            return BadRequest(MessageResponseHelper.InvalidInputParameter());
        }
    
        //TODO: check update time first - if they are the same in db -> return ~ do nothing
        
        var result = await _googleService.UpdateEvent(calendarId,eventId, accessToken, requestBody);
        if(result.IsSuccess){
            return StatusCode(StatusCodes.Status200OK, result);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
        
    }
    [HttpDelete("{email}/event/{eventId}")]
    public async Task<IActionResult> DeleteEvent(string email, string eventId, [FromQuery] string accessToken)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(accessToken))
        {
            return BadRequest(MessageResponseHelper.InvalidInputParameter());
        }
        
        var result = await _googleService.DeleteEvent(email,eventId, accessToken);
        if(result.IsSuccess){
            //TODO: delete event in db ~ update status to canceled
            
            return StatusCode(StatusCodes.Status204NoContent);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
        
    }
}