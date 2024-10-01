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

        //! Do not modify
        // [HttpGet("login")]
        // [EndpointSummary("Mentor login by Google account")]
        // public IActionResult Login()
        // {
        //     var gScopes = _configuration.GetSection("googleOAuthConfig:GScopes").Get<Dictionary<string, string?>>()!;
        //     var props = new AuthenticationProperties
        //     {
        //         RedirectUri = "/api/auth/signin-google",
        //         Items =
        //             {
        //                 { "prompt", gScopes["prompt"] },
        //                 { "access_type", gScopes["access_type"] },
        //                 { "scope", gScopes["scope"] }
        //             }
        //     };
        //     return Challenge(props, GoogleDefaults.AuthenticationScheme);
        // }
        //
        //
        // [HttpGet("signin-google")]
        // [EndpointSummary("Google call back uri")]
        // public async Task<ActionResult<BaseModel<ExternalSignInResponseModel, ExternalSignInRequestModel>>>
        //     SignInAndSignUpByGoogle()
        // {
        //     var googleAuthResponse = await _googleService.AuthenticateGoogleUserAsync(HttpContext);
        //     if (googleAuthResponse == null)
        //         return new BaseModel<ExternalSignInResponseModel, ExternalSignInRequestModel>
        //         {
        //             Message = MessageResponseHelper.AuthorizeFail(),
        //             StatusCode = StatusCodes.Status500InternalServerError,
        //             IsSuccess = false,
        //             RequestModel = null,
        //         };
        //     var response = await _authService.LoginOrSignUpExternal(new ExternalSignInRequestModel()
        //     {
        //         ExternalInfo = googleAuthResponse
        //     });
        //
        //     return StatusCode(response.StatusCode, response);
        // }
        
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        { 
            var authUrl = _googleService.GenerateOauthUrl();
           return Redirect(authUrl);
        }

        // Step 2: Endpoint to handle the Google response
        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleResponse(string code)
        {
            var tokenResponse = await _googleService.GetTokenGoogleUserAsync(code);
            if (tokenResponse == null)
            {
                return BadRequest("Error retrieving token.");
            }

            // Optionally, you can now retrieve user information using the access token
            // You may want to return the token or user info as needed

            return Ok(tokenResponse);
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
