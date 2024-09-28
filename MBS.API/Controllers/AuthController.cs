using Azure.Messaging;
using MBS.Application.Helpers;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MBS.Shared.Services.Interfaces;
using MBS.Shared.Services.Implements;

namespace MBS.API.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClaimService _claimService;
        private readonly IGoogleService _googleService;
        public AuthController(IGoogleService googleService, IClaimService claimService, IAuthService authService)
        {
            _googleService = googleService;
            _claimService = claimService;
            _authService = authService;
        }


        [HttpGet("login")]
        [EndpointSummary("Mentor login by Google account")]
        public IActionResult Login()
        {
            //var props = new AuthenticationProperties { RedirectUri = "/api/auth/signin-google" };
            var props = new AuthenticationProperties
            {
                RedirectUri = "/api/auth/signin-google",
                Items =
                    {
                        { "prompt", "consent" },
                        { "access_type", "offline" },
                        { "scope", "https://www.googleapis.com/auth/userinfo.email" +
                                    " https://www.googleapis.com/auth/userinfo.profile" +
                                    " https://www.googleapis.com/auth/calendar"
                        }
                    }
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("signin-google")]
        [EndpointSummary("Google call back uri")]
        public async Task<ActionResult<BaseModel<ExternalSignInResponseModel, ExternalSignInRequestModel>>>
            SignInAndSignUpByGoogle()
        {
            var googleAuthResponse = await _googleService.AuthenticateGoogleUserAsync(HttpContext);
            if (googleAuthResponse == null)
                return new BaseModel<ExternalSignInResponseModel, ExternalSignInRequestModel>
                {
                    Message = MessageResponseHelper.AuthorizeFail(),
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false,
                    RequestModel = null,
                };
            var response = await _authService.LoginOrSignUpExternal(new ExternalSignInRequestModel()
            {
                ExternalInfo = googleAuthResponse
            });

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
