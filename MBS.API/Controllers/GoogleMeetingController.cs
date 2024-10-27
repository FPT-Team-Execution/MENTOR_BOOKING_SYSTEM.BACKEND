using MBS.Shared.Models.Google.GoogleCalendar.Response;
using MBS.Shared.Services.Interfaces;

namespace MBS.API.Controllers;

[Route("api/google-meetings")]
//* not show google meeting api
[ApiExplorerSettings(IgnoreApi = true)]
public class GoogleMeetingController : ControllerBase
{
    private readonly IGoogleService _googleService;
    public GoogleMeetingController(IGoogleService googleService)
    {
        _googleService = googleService;
    }
    [HttpPost("")]
    public async Task<IActionResult> GetFreeBusy(string accessToken)
    {
        var result = await _googleService.CreateMeeting(accessToken);
        if(result.IsSuccess){
            return StatusCode(StatusCodes.Status200OK, result);
        }
        //error
        return StatusCode(((GoogleErrorResponse)result).Error.Code, (GoogleErrorResponse)result);
    }
}