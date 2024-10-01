using System.Net;
using MBS.Application.Helpers;
using MBS.Shared.Models.Google.GoogleOAuth.Response;
using MBS.Shared.Services.Interfaces;


namespace MBS.API.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClaimService _claimService;
        private readonly IGoogleService _googleService;
        private readonly IConfiguration _configuration;
        public AuthController(IGoogleService googleService, IClaimService claimService, IAuthService authService, IConfiguration configuration)
        {
            _googleService = googleService;
            _claimService = claimService;
            _authService = authService;
            _configuration = configuration;
        }
        
        [HttpGet("google-signin")]
        [EndpointSummary("Mentor login by Google account")]
        public IActionResult GoogleLogin()
        { 
            var authUrl = _googleService.GenerateOauthUrl();
            return Redirect(authUrl);
        }
        
        [HttpGet("signin-google")]
        [EndpointSummary("Google call back uri")]
        public async Task<IActionResult> GoogleResponse(string code)
        {
            //Get Provider Info
            // var googleAuthResponse = await _googleService.AuthenticateGoogleUserAsync(HttpContext);
            // if (!googleAuthResponse.IsSuccess)
            // {
            //     return StatusCode(StatusCodes.Status401Unauthorized, new BaseModel
            //     {
            //         Message = MessageResponseHelper.GetFailed("provider"),
            //         IsSuccess = false,
            //         StatusCode = StatusCodes.Status401Unauthorized
            //     });
            // }
            //get auth token
            var tokenResponse = await _googleService.GetTokenGoogleUserAsync(code);
            if (!tokenResponse.IsSuccess)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new BaseModel
                {
                    Message = MessageResponseHelper.AuthorizeFail(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            //get profile
            var profileResponse = await _googleService.GetProfileGoogleUserAsync(((GoogleTokenResponse)tokenResponse).access_token);
            if (!profileResponse.IsSuccess)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new BaseModel
                {
                    Message = MessageResponseHelper.AuthorizeFail(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            
            
            //SignUp Or Sign In 
            var request = new ExternalSignInRequestModel
            {
                token = (GoogleTokenResponse)tokenResponse,
                profile = (GoogleUserInfoResponse)profileResponse,
            };
            var response = await _authService.LoginOrSignUpExternal(request);

            return StatusCode(response.StatusCode, response);
        }
        
        
        [AllowAnonymous]
        [HttpPost]
        [Route("sign-up")]
        public async Task<ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>>
        SignUpStudent(
            [FromBody] RegisterRequestModel request)
        {
            var response = await _authService.SignUpAsync(request);
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

}
