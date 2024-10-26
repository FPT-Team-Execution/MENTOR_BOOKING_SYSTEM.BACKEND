using System.ComponentModel.DataAnnotations;
using System.Net;
using MBS.Application.Helpers;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Models.Google.GoogleOAuth.Response;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace MBS.API.Controllers
{
    [AllowAnonymous]
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMentorRepository _mentorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService _authService;
        private readonly IClaimService _claimService;
        private readonly IGoogleService _googleService;
        private readonly IConfiguration _configuration;
        public AuthController(
            IMentorRepository mentorRepository,
            IGoogleService googleService, 
            IClaimService claimService, 
            IAuthService authService, 
            IConfiguration configuration)
        {
            _mentorRepository = mentorRepository;
            _googleService = googleService;
            _claimService = claimService;
            _authService = authService;
            _configuration = configuration;

        }
        
        [HttpGet("google/signin")]
        [EndpointSummary("Mentor login by Google account")]
        public IActionResult GoogleLogin()
        { 
            var authUrl = _googleService.GenerateOauthUrl();
            return Redirect(authUrl);
        }
        
        [HttpGet("signin-google")]  
        [EndpointSummary("Google call back uri")]
        [ProducesResponseType(typeof(BaseModel<ExternalSignInResponseModel>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GoogleResponse([Required] string code, [Required] string callbackUri)
        {
            // Get Provider Info
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
            var tokenResponse = await _googleService.GetTokenGoogleUserAsync(code, callbackUri);
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
            
            
            //TODO: return right json 
			return StatusCode(response.StatusCode, response);
		}
        
        [HttpPost]
        [Route("sign-up")]
        [ProducesResponseType(typeof(ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>> SignUpStudent([FromBody] RegisterRequestModel request)
        {
            var response = await _authService.SignUpAsync(request);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost]
        [Route("sign-in")]
        [ProducesResponseType(typeof(ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseModel<SignInResponseModel, SignInRequestModel>>> SignIn(SignInRequestModel request)
        {
            var response = await _authService.SignIn(request);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost]
        [Route("refresh")]
        [ProducesResponseType(typeof(ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>>> Refresh(
            GetRefreshTokenRequestModel request)
        {
            var response = await _authService.Refresh(request);
            return StatusCode(response.StatusCode, response);
        }

 
        [HttpPut]
        [Route("confirm-email")]
        [ProducesResponseType(typeof(ActionResult<BaseModel<RegisterResponseModel, RegisterRequestModel>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>>> ConfirmEmail(
            ConfirmEmailRequestModel request)
        {
            var response = await _authService.ConfirmEmailAsync(request);
            return StatusCode(response.StatusCode, response);
        }
    }

}
