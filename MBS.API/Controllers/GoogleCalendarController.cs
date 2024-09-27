using MBS.Application.Helpers;
using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.Payload;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers;

[Route("api/calendar")]
[ApiController]
public class GoogleCalendarController : ControllerBase
{
    private readonly IGoogleService _googleService;

    public GoogleCalendarController(IGoogleService googleService)
    {
        _googleService = googleService;
    }

    [HttpGet("{email}/events")]
    public async Task<ActionResult<BaseModel<List<GoogleCalendarEvent>, GetGoogleCalendarEventsRequest>>> ListEvents(string email, string accessToken, DateTime timeMax, DateTime timeMin)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(accessToken) || timeMin >= timeMax)
        {
            return BadRequest(new BaseModel<List<GoogleCalendarEvent>, GetGoogleCalendarEventsRequest>
            {
                Message = MessageResponseHelper.InvalidInputParameter(),
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                RequestModel = null
            });
        }

        var request = new GetGoogleCalendarEventsRequest
        {
            Email = email,
            AccessToken = accessToken,
            TimeMax = timeMax,
            TimeMin = timeMin
        };

        var events = await _googleService.ListEvents(request);
        if (events == null)
        {
            return Unauthorized(new BaseModel<List<GoogleCalendarEvent>, GetGoogleCalendarEventsRequest>
            {
                Message = MessageResponseHelper.AuthorizeFail(),
                IsSuccess = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                RequestModel = request
            });
        }

        var response = new BaseModel<List<GoogleCalendarEvent>, GetGoogleCalendarEventsRequest>
        {
            Message = MessageResponseHelper.GetSuccessfully("events"),
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            RequestModel = request,
            ResponseModel = events
        };

        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("{email}/events")]
    public async Task<ActionResult<BaseModel<GoogleCalendarEvent, CreateGoogleCalendarEventRequest>>> InsertEvent(string email, [FromQuery] string accessToken, [FromBody]CreateGoogleCalendarEventRequest requestBody)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(accessToken) || requestBody.End.DateTime <= requestBody.Start.DateTime )
        {
            return BadRequest(new BaseModel<GoogleCalendarEvent, CreateGoogleCalendarEventRequest>
            {
                Message = MessageResponseHelper.InvalidInputParameter(),
                IsSuccess = false,
                StatusCode = StatusCodes.Status400BadRequest,
                RequestModel = null
            });
        }

        var eventCreated = await _googleService.InsertEvent(email, accessToken, requestBody);
        if (eventCreated == null)
        {
            return Unauthorized(new BaseModel<GoogleCalendarEvent, CreateGoogleCalendarEventRequest>
            {
                Message = MessageResponseHelper.AuthorizeFail(),
                IsSuccess = false,
                StatusCode = StatusCodes.Status401Unauthorized,
                RequestModel = requestBody
            });
        }

        var response = new BaseModel<GoogleCalendarEvent, CreateGoogleCalendarEventRequest>
        {
            Message = MessageResponseHelper.GetSuccessfully("events"),
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            RequestModel = null,
            ResponseModel = eventCreated
        };

        return StatusCode(response.StatusCode, response);
    }
}
