using System.ComponentModel.DataAnnotations;
using MBS.Application.Models.General;
using MBS.Shared.Models.Google;
using MBS.Shared.Models.Google.GoogleOAuth.Response;

namespace MBS.Application.Models.User;

public class ExternalSignInRequestModel
{
    public GoogleAuthResponse authenticationResult { get; set; }
    public GoogleTokenResponse token { get; set; }
    public GoogleUserInfoResponse profile { get; set; }

}

public class ExternalSignInResponseModel
{
    public  JwtModel JwtModel { get; set; }
    public GoogleTokenResponse GoogleToken { get; set; }
}