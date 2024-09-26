using Azure.Messaging;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MBS.Shared.Services.Interfaces;

namespace MBS.API.Controllers
{
    [Route("/api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IClaimService _claimService;
        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        //private readonly ISendMailService _sendMailService;
        public AuthController(IGoogleAuthenticationService googleAuthenticationService, IClaimService claimService, IAuthService authService)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _claimService = claimService;
            _authService = authService;
        }


        [HttpGet("login")]
        [ProducesResponseType(typeof(ChallengeResult), statusCode: StatusCodes.Status200OK)]
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
                                    " https://www.googleapis.com/auth/userinfo.profile" 
                        }
                    }
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("signin-google")]
        [ProducesResponseType(typeof(ContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignInAndSignUpByGoogle()
        {
            //var responseData = new object();
            var googleAuthResponse = await _googleAuthenticationService.AuthenticateGoogleUser(HttpContext);
            //            var accountByEmailResponse = await _accountService.GetAccountByEmail(googleAuthResponse.Email);
            //            if (accountByEmailResponse == null)
            //            {
            //                var createAccountResponse = await _accountService.CreateNewUserAccountByGoogle(googleAuthResponse);
            //                if (createAccountResponse == null)
            //                {
            //                    _logger.LogError("Create new user account failed with account");
            //                    return Problem(MessageConstant.Account.CreateUserAccountFailMessage);
            //                }
            //                else
            //                {
            //                    _logger.LogInformation("Create new user account successful with account");
            //                    responseData = new
            //                    {
            //                        name = createAccountResponse.Name,
            //                        email = createAccountResponse.Email,
            //                        token = await _accountService.CreateTokenByEmail(createAccountResponse.Email),
            //                        refreshToken = createAccountResponse.RefreshToken
            //                    };
            //                }
            //            }
            //            else
            //            {
            //                responseData = new
            //                {
            //                    name = accountByEmailResponse.Name,
            //                    email = accountByEmailResponse.Email,
            //                    token = await _accountService.CreateTokenByEmail(googleAuthResponse.Email),
            //                    refreshToken = await _accountService.CreateRefreshToken(googleAuthResponse.Email)
            //                };
            //            }
            //            var jsonData = JsonSerializer.Serialize(responseData);
            //            _logger.LogInformation($"{googleAuthResponse.AccessToken}");
            //            var responseHtml = $@"
            //<html>
            //    <body>
            //        <script>
            //            var authData = {jsonData};
            //            if (window.opener) {{
            //                window.opener.postMessage(authData, '*');
            //            }}
            //            window.close();
            //        </script>
            //    </body>
            //</html>";

            //var responseHtml = $@"
            //<html>
            //    <body>
            //        <script>
            //            var authData = {"jsonData"};
            //            if (window.opener) {{
            //                window.opener.postMessage(authData, '*');
            //            }}
            //            window.close();
            //        </script>
            //    </body>
            //</html>";
            //return Content(responseHtml, "text/html");
            return Ok(googleAuthResponse);
            //return Ok("Back");
        }

        [HttpGet("demo-google-claim")]
        [ProducesResponseType(typeof(ContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGoogleUserClaim()
        {
            var rs = _claimService.GetCookieValue("Google.AccessToken");
            var rsE = _claimService.GetCookieExpiredTime("Google.AccessToken");
            return Ok(new
            {
                rs,
                rsE
            });
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

}
