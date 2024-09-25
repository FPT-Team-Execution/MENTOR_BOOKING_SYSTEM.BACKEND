namespace MBS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [AllowAnonymous]
    [HttpPost]
    [Route("student/sign-up")]
    public async Task<ActionResult<BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>>>
        SignUpStudent(
            [FromBody] RegisterStudentRequestModel request)
    {
        var response = await _authService.SignUpStudentAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("mentor/sign-up")]
    public async Task<ActionResult<BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>>>
        SignUpStudent(
            [FromBody] RegisterMentorRequestModel request)
    {
        var response = await _authService.SignUpMentorAsync(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("sign-in")]
    public async Task<ActionResult<BaseModel<SignInResponseModel, SignInRequestModel>>> SignIn(
        SignInRequestModel request)
    {
        var response = await _authService.SignIn(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("refresh")]
    public async Task<ActionResult<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>>> Refresh(
        GetRefreshTokenRequestModel request)
    {
        var response = await _authService.Refresh(request);
        return StatusCode(response.StatusCode, response);
    }

    [AllowAnonymous]
    [HttpPut]
    [Route("confirm-email")]
    public async Task<ActionResult<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>>> ConfirmEmail(
        ConfirmEmailRequestModel request)
    {
        var response = await _authService.ConfirmEmailAsync(request);
        return StatusCode(response.StatusCode, response);
    }
}